using System;
using Godot;
using SecretPlan.Generated;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin;

public partial class ConfirmModal : PopupController
{
    private bool _confirmByDefault;
    private readonly CachedNode<Label> _areYouSure = new("Panel/VBoxContainer/AreYouSureLabel");
    private readonly CachedNode<Label> _contextLabel = new("Panel/VBoxContainer/ContextLabel");
    private readonly CachedNode<Button> _denyButton = new("Panel/VBoxContainer/HBoxContainer/Deny");
    private readonly CachedNode<Button> _confirmButton = new("Panel/VBoxContainer/HBoxContainer/Confirm");
    private Action? _onConfirm;

    public void Initialize(TranslatedString text, bool defaultConfirm, Action onConfirm)
    {
        _contextLabel.Get(this).Text = text;
        // _areYouSure.Get(this).Text = CoreState.TranslatedFromId(LocalizationTableIds.pause_menu__save_data__are_you_sure);
        _areYouSure.Get(this).Visible = false;
        _confirmByDefault = defaultConfirm;
        _onConfirm = onConfirm;
    }

    public override Control? GetDefaultFocusNode()
    {
        if (_confirmByDefault)
        {
            return _confirmButton.Get(this);
        }

        return _denyButton.Get(this);
    }

    protected override void OnPopupOpened()
    {
        _confirmButton.Get(this).Pressed += OnConfirm;
    }

    private void OnConfirm()
    {
        _onConfirm?.Invoke();
        ClosePopup();
    }

    protected override void OnPopupClosed()
    {
    }

    public override void AfterProcess(double delta)
    {
    }
}