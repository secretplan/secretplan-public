using System;
using System.Collections.Generic;
using Godot;
using SecretPlanCore.Core;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin;

public class PopupManager
{
    private readonly Stack<PopupShell> _popupStack = new();
    public bool IsPopupOpen => _popupStack.Count > 0;

    public PopupController? TopPopup
    {
        get
        {
            if (_popupStack.TryPeek(out var shell))
            {
                if (shell.IsValidAndNotQueuedForDeletion())
                {
                    return shell.Popup;
                }
            }

            return null;
        }
    }

    public event Action<PopupController, Control?>? PopupRequested;

    public PopupController OpenPopup(PackedScene popupPrefab, Node? owner, Control? focusReturn)
    {
        PerfClamps.Start("open_popup");
        PerfClamps.Start("instantiate");
        var popupInstance = popupPrefab.Instantiate<PopupController>();
        PerfClamps.End("instantiate");

        popupInstance.Manager = this;

        if (owner != null)
        {
            popupInstance.SetLifetimeOwner(owner);
        }

        if (focusReturn != null)
        {
            popupInstance.SetFocusReturn(focusReturn);
        }

        if (popupInstance == null)
        {
            throw new Exception($"Could not instantiate {popupPrefab} as {nameof(PopupController)}");
        }
        
        PopupRequested?.Invoke(popupInstance, focusReturn);
        PerfClamps.End("open_popup");
        return popupInstance;
    }

    public T OpenPopup<T>(CachedPackedScene<T> popup, Node? owner, Control? focusReturn) where T : PopupController
    {
        if (OpenPopup(popup.GetOrLoad(), owner, focusReturn) is T popupInstance)
        {
            return popupInstance;
        }

        throw new Exception($"Failed to instantiate {popup.Path} as {typeof(T).Name}");
    }

    public void Push(PopupShell popupShell)
    {
        _popupStack.Push(popupShell);
    }

    public void Pop()
    {
        _popupStack.Pop();
    }

    public void MarkOpened(PopupController popup)
    {
        popup.MarkPopupOpened();
    }

    public bool TryPeek(out PopupShell? controller)
    {
        return _popupStack.TryPeek(out controller);
    }
}