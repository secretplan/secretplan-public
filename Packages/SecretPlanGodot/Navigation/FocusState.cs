using BirdGame.UI;
using Godot;
using SecretPlanCore.Core;
using SecretPlanGodot.Core;
using SecretPlanGodot.Navigation;

namespace BirdGame.Core;

public class FocusState
{
    private readonly LinkedList<INavigationOwner> _previousNavigationOwners = new();
    private BoolProvider _shouldLog = new(false);
    private ViewportWrapper _viewport = new();

    public INavigationOwner? CurrentNavigationOwner
    {
        get
        {
            var topmost = _previousNavigationOwners.LastOrDefault();

            if (topmost.IsValidAndVisible())
            {
                return topmost;
            }

            while (!topmost.IsValidAndVisible())
            {
                if (_previousNavigationOwners.Count == 0)
                {
                    return null;
                }

                _previousNavigationOwners.RemoveLast();
                topmost = _previousNavigationOwners.LastOrDefault();
            }

            return topmost;
        }
    }

    public void Setup(BoolProvider shouldLog, Viewport viewport)
    {
        _viewport = new ViewportWrapper(viewport);
        _shouldLog = shouldLog;
    }

    public void FocusDefaultNodeInCurrentNavigationGroup()
    {
        if (!CurrentNavigationOwner.IsValidAndVisible())
        {
            return;
        }

        var defaultNode = CurrentNavigationOwner.GetDefaultFocusNode();

        if (defaultNode.IsValidAndNotQueuedForDeletion())
        {
            if (_shouldLog)
            {
                LocalClient.Print(
                    $"NAVI: {CurrentNavigationOwner.DebugName()} is grabbing focus with {defaultNode.Name}");
            }

            SetNodeAsFocused(defaultNode);
            return;
        }

        if (HasSomethingInFocus())
        {
            if (_shouldLog)
            {
                LocalClient.Print(
                    $"NAVI: {CurrentNavigationOwner.DebugName()} clearing focus because it has no default node");
            }

            _viewport.ReleaseFocus();
        }
    }


    /// <summary>
    ///     True if something is in focus
    /// </summary>
    public bool HasSomethingInFocus()
    {
        return _viewport.GetFocusOwner().IsValidAndNotQueuedForDeletion();
    }

    public void SetNavigationOwner(INavigationOwner? navigationOwner)
    {
        if (navigationOwner != null)
        {
            PushNavigationOwner(navigationOwner);
        }

        NavigationOwnerChanged?.Invoke(navigationOwner);
    }

    public event Action<INavigationOwner?>? NavigationOwnerChanged;

    private void PushNavigationOwner(INavigationOwner owner)
    {
        if (_shouldLog)
        {
            LocalClient.Print($"NAVI: New Navigation Owner {owner.DebugName()}");
        }

        // If this owner is already in the stack, remove it
        _previousNavigationOwners.Remove(owner);

        // Append it to the end of the stack
        _previousNavigationOwners.AddLast(owner);
    }

    private static void SetNodeAsFocused(Control defaultFocusNode)
    {
        if (defaultFocusNode.GetFocusModeWithOverride() != Control.FocusModeEnum.None)
        {
            defaultFocusNode.CallDeferred(Control.MethodName.GrabFocus);
        }
        else
        {
            LocalClient.Error($"Could not grab focus! {defaultFocusNode.Name} has FocusMode = None");
        }
    }
}