using DATA_ASSEMBLY.Distributable;
using DATA_ASSEMBLY.DistributableConfig;
using FriendShapedDistributable;
using Godot;
using SecretPlan.Generated;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin;

[GlobalClass]
public partial class LocalizedParent : Node
{
    private readonly ParentCore _core = new();

    [Export]
    private LocalizationTableIds _localizationId;

    private LocalizedStringReference _stringReference;
    private CoreState CoreState => _core.State(this);

    public override void _EnterTree()
    {
        CallDeferred(nameof(Setup));
        CoreState.SerializedState.Settings.LocaleChanged += OnLocaleChanged;
    }

    public override void _ExitTree()
    {
        CoreState.SerializedState.Settings.LocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged()
    {
        Refresh();
    }

    private void Setup()
    {
        _stringReference = LocalizationServer.Instance.GetReferenceFromId((uint)_localizationId);
        Refresh();
    }

    private void Refresh()
    {
        SetParentText(_stringReference.Translated(CoreState.SerializedState.Settings.Locale));
    }

    private void SetParentText(TranslatedString translated)
    {
        var parent = GetParent();
        if (parent is Button button)
        {
            button.Text = translated;
        }

        if (parent is Label label)
        {
            label.Text = translated;
        }

        if (parent is Label3D label3D)
        {
            label3D.Text = translated;
        }

        if (parent is LineEdit lineEdit)
        {
            lineEdit.PlaceholderText = translated;
        }

        if (parent is RichTextLabel richTextLabel)
        {
            richTextLabel.Text = translated;
        }
    }
}