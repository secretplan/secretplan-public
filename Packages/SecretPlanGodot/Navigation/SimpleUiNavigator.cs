using BirdGame.UI;
using Godot;
using SecretPlanGodot.Core;

namespace BirdGame.Core;

/// <summary>
///     Doesn't do anything special for navigation, leans on Godot's default behavior
/// </summary>
public class SimpleUiNavigator : IUiNavigator
{
    public void OnFocusChanged(Control newlyFocusedControl)
    {
    }

    public void OnNavigationModeChanged(NavigationMode navigationMode)
    {
    }

    public void SetNavigationOwner(INavigationOwner? control)
    {
    }

    public void UpdateHoverAndFocus(NavigationMode navigationMode)
    {
    }
}