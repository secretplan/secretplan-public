using System;
using System.Linq;
using DATA_ASSEMBLY.Distributable;
using DATA_ASSEMBLY.DistributableConfig;
using FriendShapedDistributable;
using Godot;
using SecretPlan.Generated;
using SecretPlanGodot.ConfigEditor;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class LocStringReferenceLink : Control
{
    private readonly CachedNode<Label> _label = new("SizeProxy/SizeProxyDriver/SlugLabel");
    private readonly CachedNode<Button> _linkButton = new("SizeProxy/Button");

    private readonly ParentCore _parentCore = new();

    private readonly CachedPackedScene<LocStringSearchPopup> _popup =
        new("res://FriendShapedPlugin/ConfigEditor/Scenes/SearchLocStringsPopup.tscn");

    private ConfigField? _field;
    private Action<LocalizedStringReference>? _onRefresh;

    private Label Label => _label.Get(this);
    private Button LinkButton => _linkButton.Get(this);
    private CoreState CoreState => _parentCore.State(this);

    public void Initialize(ConfigField field, Action<LocalizedStringReference> onRefresh)
    {
        _field = field;
        _onRefresh = onRefresh;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        var reference = GetLocStringReference();

        if (reference.HasValue)
        {
            Label.Text = reference.Value.Slug;
        }
        else
        {
            Label.Text = "ERROR";
        }

        _onRefresh?.Invoke(reference ?? new LocalizedStringReference());
    }

    private LocalizedStringReference? GetLocStringReference()
    {
        var value = _field?.GetValue();

        if (value is LocalizedStringReference localizedStringReference)
        {
            return localizedStringReference;
        }

        LocalClient.Error($"{nameof(LocStringReferenceLink)} could not initialize value: {value}");
        return null;
    }

    public override void _EnterTree()
    {
        LinkButton.Pressed += LinkButtonPressed;
    }

    public override void _ExitTree()
    {
        LinkButton.Pressed -= LinkButtonPressed;
    }

    private void LinkButtonPressed()
    {
        var reference = GetLocStringReference();

        if (reference.HasValue)
        {
            var allPossibleResults = LocalizationServer.Instance.AllIds().ToList();

            CoreState.PopupManager.OpenPopup(_popup, this, LinkButton)
                .Initialize(OnSelected, reference.Value.Id, allPossibleResults,
                    id =>
                    {
                        var foundReference = LocalizationServer.Instance.GetReferenceFromId(id);
                        return foundReference.Slug;
                    }, true, reference.Value.Slug);
        }
    }

    private void OnSelected(uint locStringId)
    {
        if (_field == null)
        {
            return;
        }

        _field.SetValue(new LocalizedStringReference(locStringId));
        UpdateDisplay();
    }
}