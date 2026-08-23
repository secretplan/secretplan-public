using FriendShapedDistributable;
using Godot;
using SecretPlan.Generated;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin;

public partial class PopupCloseButton : Button
{
    private readonly ParentCore _core = new();
    private readonly CachedAncestor<PopupController> _popup = new();
    private Vector2 _minSize;
    private CoreState CoreState => _core.State(this);

    public override void _EnterTree()
    {
        Pressed += OnPressed;
        CoreState.SerializedState.Settings.LocaleChanged += OnLocaleChanged;
    }
    
    public override void _ExitTree()
    {
        CoreState.SerializedState.Settings.LocaleChanged -= OnLocaleChanged;
    }

    public override void _Process(double delta)
    {
        Visible = CoreState.PopupManager.TopPopup == _popup.GetOrNull(this);
    }
    
    private void OnLocaleChanged()
    {
        if (Text != string.Empty)
        {
            Text = CoreState.TranslatedFromId(LocalizationTableIds.settings__off);
        }
    }

    private void OnPressed()
    {
        if (!Visible)
        {
            // If the button is not visible, it doesn't work
            return;
        }

        _popup.GetOrNull(this)?.ClosePopup();
    }
}