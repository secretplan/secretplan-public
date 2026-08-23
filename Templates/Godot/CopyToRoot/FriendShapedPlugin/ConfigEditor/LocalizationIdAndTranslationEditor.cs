using System.Linq;
using DATA_ASSEMBLY.DistributableConfig;
using FriendShapedDistributable;
using Godot;
using SecretPlanGodot.ConfigEditor;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class LocalizationIdAndTranslationEditor : FieldEditor, ICanNotifyOfCleaned
{
    private readonly CachedNode<Button> _button = new("HBoxContainer/Button");
    private readonly ParentCore _parentCore = new();

    private readonly CachedPackedScene<LocStringSearchPopup> _popup =
        new("res://FriendShapedPlugin/Scenes/ConfigEditor/SearchLocStringsPopup.tscn");

    private readonly CachedNode<Label> _slugLabel = new("HBoxContainer/Label");
    private readonly CachedNode<StringEditor> _translationEditor = new("TranslationEditor");
    private ConfigField? _configField;

    private ConfigField? _idSubfield;

    private StringEditor TranslationEditor => _translationEditor.Get(this);

    private CoreState CoreState => _parentCore.State(this);

    public void OnNotifyOfClean()
    {
        UpdateDisplay();
    }

    public override void Initialize(ConfigField configField)
    {
        _configField = configField;

        foreach (var subField in _configField.GetSubfields())
        {
            if (subField.RealMemberName == nameof(LocalizationExtensionIdAndTranslation.Translation))
            {
                if (subField.GetValueOrDefaultAs<string>() == null)
                {
                    subField.SetValue(string.Empty);
                }

                TranslationEditor.Initialize(subField);
            }

            if (subField.RealMemberName == nameof(LocalizationExtensionIdAndTranslation.Id))
            {
                _idSubfield = subField;
            }
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (_configField == null)
        {
            return;
        }

        if (_idSubfield == null)
        {
            return;
        }

        var id = _idSubfield.GetValueOrDefaultAs<uint>();
        var slug = LocalizationServer.Instance.GetSlug(id);
        if (string.IsNullOrEmpty(slug))
        {
            slug = $"[{id}]";
        }

        _slugLabel.Get(this).Text = slug;
    }

    public override void _EnterTree()
    {
        _button.Get(this).Pressed += ButtonPressed;
    }

    public override void _ExitTree()
    {
        _button.Get(this).Pressed -= ButtonPressed;
    }

    private LocalizationExtensionIdAndTranslation? GetLocStringReference()
    {
        return _configField?.GetValueOrDefaultAs<LocalizationExtensionIdAndTranslation>();
    }

    private void ButtonPressed()
    {
        var reference = GetLocStringReference();

        if (!reference.HasValue)
        {
            return;
        }

        var allPossibleResults = LocalizationServer.Instance.AllIds().ToList();

        CoreState.PopupManager.OpenPopup(_popup, this, _button.Get(this))
            .Initialize(OnSelected, reference.Value.Id, allPossibleResults,
                id =>
                {
                    var foundReference = LocalizationServer.Instance.GetReferenceFromId(id);
                    return foundReference.Slug;
                }, true, LocalizationServer.Instance.GetSlug(reference.Value.Id));
    }

    private void OnSelected(uint newId)
    {
        if (_idSubfield == null)
        {
            return;
        }

        _idSubfield.SetValue(newId);
        UpdateDisplay();
    }
}