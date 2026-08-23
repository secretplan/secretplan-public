using Godot;
using SecretPlanGodot.ConfigEditor;
using SecretPlanGodot.Configuration;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public abstract partial class ResourceReferenceEditor<T> : FieldEditor where T : Resource
{
    private readonly CachedNode<ResourceReferenceLink> _resourceReferenceEditor = new("ResourceReferenceLink");
    private ConfigField? _field;

    public override void Initialize(ConfigField field)
    {
        _field = field;
        Refresh();

        _resourceReferenceEditor.Get(this).Initialize(field, field.AssociatedType, Refresh);
    }

    protected ResourceReference<T>? GetCurrentValue()
    {
        return _field?.GetValueAs<ResourceReference<T>>();
    }

    protected IResourceReference? GetCurrentTypeless()
    {
        return _field?.GetValue() as IResourceReference;
    }

    protected abstract void Refresh();
}