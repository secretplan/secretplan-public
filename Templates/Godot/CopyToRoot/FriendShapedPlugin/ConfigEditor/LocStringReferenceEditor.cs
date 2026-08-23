using DATA_ASSEMBLY.DistributableConfig;
using Godot;
using SecretPlan.Generated;
using SecretPlanGodot.ConfigEditor;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class LocStringReferenceEditor : FieldEditor
{
    private readonly CachedNode<LocStringReferenceLink> _link = new("LocStringLink");
    private readonly CachedNode<RichTextLabel> _textPreview = new("PreviewContainer/Preview");


    public override void Initialize(ConfigField configField)
    {
        _link.Get(this).Initialize(configField, Refresh);
    }

    private void Refresh(LocalizedStringReference localizedStringReference)
    {
        var fallbackLocale = LocalizationRootTableEnum.LocalizationTable.ReadOrDefault().FallbackLocale;
        _textPreview.Get(this).Text = $"[color=cccccc][{fallbackLocale}][/color] " +
                                      localizedStringReference.TranslatedWithFallbackLocale();
    }
}