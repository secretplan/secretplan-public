using Godot;
using SecretPlanGodot.Core;

namespace BirdGame.Core;

public interface IUiNavigator
{
    void OnFocusChanged(Control newlyFocusedControl);
    void OnNavigationModeChanged(NavigationMode navigationMode);
    void UpdateHoverAndFocus(NavigationMode navigationMode);
}