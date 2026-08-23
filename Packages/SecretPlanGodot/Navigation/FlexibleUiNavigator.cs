using BirdGame.Core;
using BirdGame.UI;
using Godot;
using SecretPlanCore.Core;
using SecretPlanGodot.Core;

namespace SecretPlanGodot.Navigation;

/// <summary>
///     Keyboard, Mouse, Controller navigator that can actively swap between all of those
/// </summary>
public class FlexibleUiNavigator : IUiNavigator
{
    /// <summary>
    ///     StyleBox when a button is hovered
    /// </summary>
    private readonly StyleBox? _buttonHoverStyleBox;

    /// <summary>
    ///     Stylebox when a button is normal
    /// </summary>
    private readonly StyleBox? _buttonNormalStyleBox;

    private readonly CachedResource<Theme> _defaultTheme = new("res://DefaultTheme.tres");
    private readonly FocusState _focusState;
    private readonly CachedResource<StyleBoxFlat> _focusStyleBox = new("res://FocusStyleBox.tres");
    private readonly Color _focusStyleBoxColor;
    private BoolProvider _shouldLog = new(false);
    private Action? _playRolloverSound;
    private ViewportWrapper _viewport = new();

    public FlexibleUiNavigator(FocusState focusState)
    {
        _focusState = focusState;
        _buttonHoverStyleBox = _defaultTheme.GetOrLoad().GetStylebox("hover", "Button");
        _buttonNormalStyleBox = _defaultTheme.GetOrLoad().GetStylebox("normal", "Button");
        _focusStyleBoxColor = _focusStyleBox.GetOrLoad().BorderColor;

        focusState.NavigationOwnerChanged += OnNavigationOwnerChanged;
    }

    public Control? LastHoveredControl { get; private set; }

    public Control? LastFocusedControl { get; private set; }

    public void OnFocusChanged(Control newlyFocusedControl)
    {
        if (!_focusState.CurrentNavigationOwner.IsValidAndVisible())
        {
            if (_shouldLog)
            {
                LocalClient.Print("NAVI: There are no navigation groups");
            }

            return;
        }

        var wasSuccessful = true;

        if (newlyFocusedControl.GetNavigationOwner() != _focusState.CurrentNavigationOwner)
        {
            if (_shouldLog)
            {
                LocalClient.Print(
                    $"NAVI: Attempted to leave navigation group ({_focusState.CurrentNavigationOwner.DebugName()} -> {newlyFocusedControl.GetNavigationOwner().DebugName()}), jumping back to previous node");
            }

            wasSuccessful = false;

            var previousNode = LastFocusedControl;

            if (previousNode.IsValidAndNotQueuedForDeletion())
            {
                if (_shouldLog)
                {
                    LocalClient.Print($"NAVI: Jumping back to previous node {previousNode.Name}");
                }

                previousNode.GrabFocus();
            }
            else
            {
                _focusState.FocusDefaultNodeInCurrentNavigationGroup();
            }
        }

        if (wasSuccessful)
        {
            if (LastFocusedControl != newlyFocusedControl)
            {
                if (newlyFocusedControl is not IFocusSink)
                {
                    _playRolloverSound?.Invoke();
                }
            }

            LastFocusedControl = newlyFocusedControl;
        }
    }

    public void OnNavigationModeChanged(NavigationMode navigationMode)
    {
        if (_shouldLog)
        {
            LocalClient.Print($"NAVI: Changed input mode: {navigationMode}");
        }

        if (navigationMode == NavigationMode.Mouse)
        {
            StyleForMouse();
        }
        else
        {
            StyleForDirectionalNavigation();
        }
    }

    public void UpdateHoverAndFocus(NavigationMode navigationMode)
    {
        UpdateHoveredControl(navigationMode);

        if (navigationMode != NavigationMode.Mouse)
        {
            // If we don't have focus, try to find something to focus on
            if (!_focusState.HasSomethingInFocus())
            {
                _focusState.FocusDefaultNodeInCurrentNavigationGroup();
            }
        }
    }

    public void StyleForDirectionalNavigation()
    {
        _focusStyleBox.GetOrLoad().BorderColor = _focusStyleBoxColor;
        _defaultTheme.GetOrLoad().SetStylebox("hover", "Button", _buttonNormalStyleBox);
    }

    public void StyleForMouse()
    {
        LastHoveredControl = null;

        _focusStyleBox.GetOrLoad().BorderColor = Colors.Transparent;
        _defaultTheme.GetOrLoad().SetStylebox("hover", "Button", _buttonHoverStyleBox);
    }

    public void Setup(BoolProvider shouldLog, Viewport viewport, Action playRolloverSound)
    {
        _viewport = new ViewportWrapper(viewport);
        _shouldLog = shouldLog;
        _playRolloverSound = playRolloverSound;
    }

    private Control? GetHoveredMeaningfulControl()
    {
        var currentHovered = _viewport.GetHoveredControl();

        if (!currentHovered.IsValidAndNotQueuedForDeletion())
        {
            // Nothing hovered, no work to do.
            return null;
        }

        if (currentHovered.GetFocusModeWithOverride() == Control.FocusModeEnum.None)
        {
            // Control is not "meaningful" (ie it's a group or a spacer or something)
            return null;
        }

        if (currentHovered is ScrollBar)
        {
            // This will cause the scroll container to be focused, not desirable
            return null;
        }

        if (currentHovered is LineEdit)
        {
            // You need to click on a LineEdit to give it focus
            return null;
        }

        return currentHovered;
    }

    private void UpdateHoveredControl(NavigationMode navigationMode)
    {
        if (navigationMode != NavigationMode.Mouse)
        {
            return;
        }

        var currentHovered = GetHoveredMeaningfulControl();

        if (currentHovered == null)
        {
            return;
        }

        if (currentHovered != LastHoveredControl)
        {
            var focusModeWithOverride = currentHovered.GetFocusModeWithOverride();
            if (focusModeWithOverride == Control.FocusModeEnum.Click ||
                focusModeWithOverride == Control.FocusModeEnum.All)
            {
                // Newly hovered thing grabs focus
                currentHovered.GrabFocus();
            }
        }

        // this should probably only happen when grab focus, idk if there's a bug here
        LastHoveredControl = currentHovered;
    }


    private void OnNavigationOwnerChanged(INavigationOwner? owner)
    {
        _focusState.FocusDefaultNodeInCurrentNavigationGroup();
    }
}

public class ViewportWrapper
{
    private readonly Viewport? _viewport;

    public ViewportWrapper()
    {
    }

    public ViewportWrapper(Viewport? viewport)
    {
        _viewport = viewport;
    }

    public Control? GetHoveredControl()
    {
        return _viewport?.GuiGetHoveredControl();
    }

    public Control? GetFocusOwner()
    {
        return _viewport?.GuiGetFocusOwner();
    }

    public void ReleaseFocus()
    {
        _viewport?.GuiReleaseFocus();
    }
}