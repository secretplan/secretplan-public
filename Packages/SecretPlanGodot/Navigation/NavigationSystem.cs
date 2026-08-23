using BirdGame.Core;
using SecretPlanCore.Core;
using SecretPlanGodot.Core;

namespace SecretPlanGodot.Navigation;

public class NavigationSystem
{
    private readonly FlexibleUiNavigator _flexibleNavigator;
    private readonly BoolProvider _shouldLog;

    private int _gamepadSupportFlag;
    private bool _hasNotifiedControllerUse;
    private IUiNavigator _underlyingUiNavigator = new SimpleUiNavigator();

    public NavigationSystem(BoolProvider shouldLog)
    {
        _flexibleNavigator = new FlexibleUiNavigator(FocusState);
        _shouldLog = shouldLog;
    }

    public NavigationMode CurrentNavigationMode { get; private set; }
    public FocusState FocusState { get; } = new();

    public IUiNavigator UiNavigator
    {
        get => _underlyingUiNavigator;
        private set
        {
            _underlyingUiNavigator = value;
            _underlyingUiNavigator.OnNavigationModeChanged(CurrentNavigationMode);
        }
    }

    public void SetGamepadSupport(bool shouldBeEnabled)
    {
        var incrementor = shouldBeEnabled ? -1 : 1;
        _gamepadSupportFlag += incrementor;

        if (_gamepadSupportFlag < 0)
        {
            _gamepadSupportFlag = 0;
        }

        if (_shouldLog)
        {
            LocalClient.Print($"NAVI: Directional navigation support flag = {_gamepadSupportFlag}");
        }

        if (_gamepadSupportFlag == 0)
        {
            UiNavigator = _flexibleNavigator;
        }
        else
        {
            _flexibleNavigator.StyleForDirectionalNavigation();
            UiNavigator = new SimpleUiNavigator();
        }
    }

    public void SetNavigationMode(NavigationMode mode)
    {
        if (CurrentNavigationMode != mode)
        {
            if (mode == NavigationMode.Gamepad && !_hasNotifiedControllerUse)
            {
                _hasNotifiedControllerUse = true;
                // send telemetry?
            }

            CurrentNavigationMode = mode;
            UiNavigator.OnNavigationModeChanged(mode);
        }
    }
}