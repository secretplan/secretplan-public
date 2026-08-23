using SecretPlanGodot.ConfigEditor;
using SecretPlanGodot.Configuration;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class Vector3Editor : FieldEditor
{
    private readonly CachedNode<NumberEditor> _x = new("X");
    private readonly CachedNode<NumberEditor> _y = new("Y");
    private readonly CachedNode<NumberEditor> _z = new("Z");
    private ConfigField? _configField;

    private NumberEditor X => _x.Get(this);
    private NumberEditor Y => _y.Get(this);
    private NumberEditor Z => _z.Get(this);

    public override void Initialize(ConfigField configField)
    {
        _configField = configField;

        foreach (var subField in _configField.GetSubfields())
        {
            if (subField.RealMemberName == nameof(SerializedVector3.X))
            {
                X.Initialize(subField);
            }

            if (subField.RealMemberName == nameof(SerializedVector3.Y))
            {
                Y.Initialize(subField);
            }

            if (subField.RealMemberName == nameof(SerializedVector3.Z))
            {
                Z.Initialize(subField);
            }
            else
            {
                Z.Visible = false;
            }
        }
    }
}