using System.Diagnostics.CodeAnalysis;
using BirdGame.Core;
using BirdGame.UI;
using FriendShapedDistributable;
using Godot;
using SecretPlanCore.Core;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin;

public abstract partial class PopupController : Control, INavigationOwner
{
    private readonly ParentCore _parentCore = new();
    private readonly BoolProvider _proxyLifetimeProvider = new(true);

    [Export]
    private bool _closeOnCancel = true;

    [Export]
    private AudioStream? _closeSound;

    [Export]
    private PackedScene? _customScrim;

    private Control? _customScrimInstance;

    [Export]
    private AudioStream? _openSound;

    protected Control? FocusReturn;

    public bool HasFinishedOpening { get; private set; }

    public AudioStream? OpenSound => _openSound;

    public AudioStream? CloseSound => _closeSound;
    public PopupManager? Manager { get; set; }
    protected CoreState CoreState => _parentCore.State(this);
    public bool CloseOnCancel => _closeOnCancel;

    public abstract Control? GetDefaultFocusNode();

    public void ClosePopup()
    {
        Visible = false;
        QueueFree();
    }

    public sealed override void _EnterTree()
    {
    }

    public sealed override void _ExitTree()
    {
        OnPopupClosed();
        _customScrimInstance?.QueueFree();

        if (FocusReturn.IsValidAndNotQueuedForDeletion())
        {
            if (CoreState.Debug.LogUiNavigation)
            {
                LocalClient.Print($"NAVI: Popup {Name} is closing, passing focus to FocusReturn: {FocusReturn.Name}");
            }

            FocusReturn.CallDeferred(Control.MethodName.GrabFocus);
        }
    }

    public void MarkPopupOpened()
    {
        if (!HasFinishedOpening)
        {
            OnPopupOpened();
            HasFinishedOpening = true;
        }
    }

    protected abstract void OnPopupOpened();
    protected abstract void OnPopupClosed();

    public sealed override void _Process(double delta)
    {
        if (!_proxyLifetimeProvider.Value)
        {
            QueueFree();
        }

        AfterProcess(delta);
    }

    public abstract void AfterProcess(double delta);

    public static bool IsPopupValid([NotNullWhen(true)] PopupController? popup)
    {
        return popup.IsValidAndNotQueuedForDeletion() && popup._proxyLifetimeProvider.Value;
    }
    
    public static bool IsPopupShellValid([NotNullWhen(true)] PopupShell? topPopupShell)
    {
        return topPopupShell.IsValidAndNotQueuedForDeletion() && IsPopupValid(topPopupShell.Popup);
    }
    

    /// <summary>
    ///     Sets the "owner" of this popup. If the owner dies, this popup dies too. (this helps ensure popups don't stick
    ///     around across scenes unless they're supposed to.
    /// </summary>
    public void SetLifetimeOwner(Node owner)
    {
        _proxyLifetimeProvider.SetProvider(owner.IsValidAndNotQueuedForDeletion);
    }

    public Control? InstantiateCustomScrim()
    {
        _customScrimInstance = _customScrim?.Instantiate<Control>();
        return _customScrimInstance;
    }

    protected Control? GetFirstButton()
    {
        foreach (var descendent in this.GetAllDescendants())
        {
            if (descendent is Button button)
            {
                if (button.GetFocusModeWithOverride() != FocusModeEnum.None && button.Visible)
                {
                    return button;
                }
            }
        }

        return null;
    }

    public void SetFocusReturn(Control returnFocus)
    {
        FocusReturn = returnFocus;
    }

    public sealed override void _Input(InputEvent inputEvent)
    {
        AfterInput(inputEvent);
    }

    protected virtual void AfterInput(InputEvent inputEvent)
    {
    }

    public sealed override void _UnhandledInput(InputEvent inputEvent)
    {
        AfterUnhandledInput(inputEvent);

        if (inputEvent.IsActionPressed(StringNameCache.UiCancel) && CloseOnCancel && HasFinishedOpening)
        {
            ClosePopup();
        }

        // catch any extra inputs so they don't propogate below this popup
        GetViewport().SetInputAsHandled();
    }

    protected virtual void AfterUnhandledInput(InputEvent inputEvent)
    {
    }
}