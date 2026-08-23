using Godot;
using SecretPlanGodot.ConfigEditor;
using SecretPlanGodot.Configuration;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class ColorEditor : FieldEditor
{
    private readonly CachedNode<ColorPicker> _colorPicker = new("ColorPicker");
    private readonly CachedNode<ColorRect> _colorRect = new("ColorRect");
    private ConfigField? _configField;

    public override void Initialize(ConfigField configField)
    {
        _configField = configField;

        if (_configField.GetValue() is not ISerializedColor color)
        {
            return;
        }

        _colorPicker.Get(this).Color = color.ToColor();
        Refresh();

        _colorPicker.Get(this).ColorChanged += ColorChanged;
    }

    private void ColorChanged(Color color)
    {
        if (_configField == null)
        {
            return;
        }

        if (_configField.GetValue() is SerializedColorRgba)
        {
            _configField.SetValue(SerializedColorRgba.FromColor(color));
        }

        if (_configField.GetValue() is SerializedColorRgb)
        {
            _configField.SetValue(SerializedColorRgb.FromColor(color));
        }

        Refresh();
    }

    private void Refresh()
    {
        if (_configField == null)
        {
            LocalClient.Error("Color Editor was not initialized");
            return;
        }

        if (_configField.GetValue() is not ISerializedColor color)
        {
            return;
        }

        _colorRect.Get(this).Color = color.ToColor();
    }
}