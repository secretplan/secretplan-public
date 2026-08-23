using Godot;
using SecretPlanGodot.ConfigEditor;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class BoolEditor : FieldEditor
{
    private readonly CachedNode<Button> _button = new("Button");
    private ConfigField? _configField;

    private Button Button => _button.Get(this);

    public override void Initialize(ConfigField configField)
    {
        _configField = configField;
        RefreshState();
    }

    private void RefreshState()
    {
        if (CurrentValue())
        {
            Button.Text = "On";
        }
        else
        {
            Button.Text = "Off";
        }
    }

    private bool CurrentValue()
    {
        if (_configField == null)
        {
            LocalClient.Print("BoolEditor was not initialized");
            return false;
        }

        if (_configField.GetValue() is not bool value)
        {
            LocalClient.Error($"{_configField} was passed to a BoolEditor, that's no bool!");
            return false;
        }

        return value;
    }

    public override void _EnterTree()
    {
        Button.Pressed += OnButtonPressed;
    }

    public override void _ExitTree()
    {
        Button.Pressed -= OnButtonPressed;
    }

    private void OnButtonPressed()
    {
        if (_configField != null)
        {
            _configField.SetValue(!CurrentValue());
        }

        RefreshState();
    }
}