using DATA_ASSEMBLY.DistributableConfig;
using Godot;
using SecretPlanCore.Configuration;
using SecretPlanGodot.ConfigEditor;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class LocalizationIdAndSlugEditor : FieldEditor
{
    private readonly CachedNode<StringEditor> _slugEditor = new("SlugEditor");
    private readonly CachedNode<Label> _idLabel = new("IdLabel");
    private ConfigField? _configField;

    private StringEditor SlugEditor => _slugEditor.Get(this);

    public override void Initialize(ConfigField configField)
    {
        _configField = configField;

        foreach (var subField in _configField.GetSubfields())
        {
            if (subField.RealMemberName == nameof(LocalizationExtensionIdAndSlug.Slug))
            {
                var slug = subField.GetValueOrDefaultAs<string>();
                if (slug == null)
                {
                    subField.SetValue(string.Empty);
                }
                
                SlugEditor.Initialize(subField);
            }

            if (subField.RealMemberName == nameof(LocalizationExtensionIdAndSlug.Id))
            {
                var id = subField.GetValueOrDefaultAs<uint>();

                if (id == 0)
                {
                    id = ConfigServer.Instance.GenerateInstanceId();
                }
                
                subField.SetValue(id);
                
                _idLabel.Get(this).Text = id.ToString();
            }
        }
    }
}