using FriendShapedDistributable;
using Godot;
using SecretPlanCore.Core;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin;

public partial class PopupTree : Control
{
    private readonly CachedNode<AudioStreamPlayer> _closeSoundPlayer = new("CloseSound");
    private readonly ParentCore _core = new();
    private readonly CachedNode<AudioStreamPlayer> _openSoundPlayer = new("OpenSound");
    private readonly CachedPackedScene<PopupShell> _popupShell = new("res://FriendShapedPlugin/Scenes/PopupShell.tscn");
    private Control? _focusReturn;
    private AudioStream? _pendingCloseSound;
    private CoreState CoreState => _core.State(this);

    public override void _EnterTree()
    {
        Visible = false;
        CoreState.PopupManager.PopupRequested += OpenPopup;
    }

    public override void _ExitTree()
    {
        if (_focusReturn.IsValidAndNotQueuedForDeletion())
        {
            CallDeferred(Control.MethodName.GrabFocus, _focusReturn);
        }

        CoreState.PopupManager.PopupRequested -= OpenPopup;
    }

    public override void _Process(double delta)
    {
        if (CoreState.PopupManager.TryPeek(out var topPopupShell))
        {
            if (PopupController.IsPopupShellValid(topPopupShell))
            {
                Visible = true;

                // popup is not null, IsPopupShellValid should have confirmed that.
                var topPopup = topPopupShell.Popup!;

                _pendingCloseSound = topPopup.CloseSound;

                if (!topPopup.HasFinishedOpening)
                {
                    CoreState.PopupManager.MarkOpened(topPopup);
                }
            }
            else
            {
                // Popup (or it's shell, or both) is invalid, pop the stack
                CoreState.PopupManager.Pop();

                topPopupShell?.QueueFree();

                _core.State(this).MouseLock.LockMouse();

                if (_pendingCloseSound != null)
                {
                    var closeSoundPlayer = _closeSoundPlayer.Get(this);
                    closeSoundPlayer.Stream = _pendingCloseSound;
                    closeSoundPlayer.Play();
                }
            }
        }
        else
        {
            Visible = false;
        }
    }

    private void OpenPopup(PopupController popupInstance, Control? focusReturn)
    {
        var shell = _popupShell.LoadAndInstantiate();
        shell.Name = $"PopupShell_{popupInstance.Name}";
        AddChild(shell);
        CoreState.PopupManager.Push(shell);
        _core.State(this).MouseLock.FreeMouse();
        PerfClamps.Start("add_child");
        shell.SetPopup(popupInstance);
        PerfClamps.End("add_child");

        if (popupInstance.OpenSound != null)
        {
            var openSoundPlayer = _openSoundPlayer.Get(this);
            openSoundPlayer.Stream = popupInstance.OpenSound;
            openSoundPlayer.Play();
        }

        var customScrim = popupInstance.InstantiateCustomScrim();
        if (customScrim != null)
        {
            shell.SetScrim(customScrim);
        }

        CoreState.NavigationSystem.FocusState.SetNavigationOwner(popupInstance);
        _focusReturn = focusReturn;
    }
}