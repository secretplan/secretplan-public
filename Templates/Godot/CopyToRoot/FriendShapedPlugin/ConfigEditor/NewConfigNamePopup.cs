using System;
using Godot;
using SecretPlanCore.Configuration;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class NewConfigNamePopup : PopupController
{
    private readonly CachedNode<LineEdit> _lineEdit = new("Root/Stack/LineEdit");
    private readonly CachedNode<Label> _titleLabel = new("Root/Stack/HBoxContainer/Label");
    private Type? _configType;
    private ConfigEditor? _editor;
    private Action<string>? _onSubmit;

    public void Initialize(string title, Type configType, ConfigEditor editor, Action<string>? onSubmit, string? preFilledText = null)
    {
        _editor = editor;
        _titleLabel.Get(this).Text = title;
        _onSubmit = onSubmit;
        _configType = configType;
        if (preFilledText != null)
        {
            _lineEdit.Get(this).Text = preFilledText;
            _lineEdit.Get(this).SelectAll();
            _lineEdit.Get(this).CaretColumn = _lineEdit.Get(this).Text.Length;
        }
    }

    private void OnSubmit(string newText)
    {
        if (_configType == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(newText))
        {
            return;
        }

        if (!ConfigServer.Instance.IsNameAvailable(ConfigServer.Instance.CreateFileName(_configType, newText)))
        {
            LocalClient.Error($"Attempted to make config instance called {newText}, but that name is taken");
            return;
        }

        _onSubmit?.Invoke(newText);
        ClosePopup();
    }

    protected override void OnPopupOpened()
    {
        _lineEdit.Get(this).TextSubmitted += OnSubmit;
        _lineEdit.Get(this)
            .CallDeferred(Control.MethodName.GrabFocus); // todo: can maybe remove this with the new focus system?
    }

    protected override void OnPopupClosed()
    {
    }

    public override void AfterProcess(double delta)
    {
    }

    public override Control GetDefaultFocusNode()
    {
        return _lineEdit.Get(this);
    }
}