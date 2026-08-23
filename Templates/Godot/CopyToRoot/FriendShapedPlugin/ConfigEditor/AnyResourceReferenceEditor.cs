using SecretPlanGodot.ConfigEditor;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

/// <summary>
///     Used for ResourceReference editors that don't have a custom editor
/// </summary>
public partial class AnyResourceReferenceEditor : FieldEditor
{
    private readonly CachedNode<ResourceReferenceLink> _resourceReferenceEditor = new("ResourceReferenceLink");

    public override void Initialize(ConfigField field)
    {
        _resourceReferenceEditor.Get(this).Initialize(field, field.AssociatedType, Refresh);
    }

    private void Refresh()
    {
        // doesn't need to do anything
    }
}