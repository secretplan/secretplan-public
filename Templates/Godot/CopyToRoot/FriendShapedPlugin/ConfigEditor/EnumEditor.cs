using System;
using System.Collections.Generic;
using Godot;
using SecretPlanGodot.ConfigEditor;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class EnumEditor : FieldEditor
{
    private readonly CachedNode<OptionButton> _dropdown = new("OptionButton");
    private readonly Dictionary<int, long> _indexToValue = new();
    private readonly Dictionary<long, int> _valueToIndex = new();
    private ConfigField? _configField;
    private OptionButton Dropdown => _dropdown.Get(this);

    public override void Initialize(ConfigField configField)
    {
        _configField = configField;

        var currentValueAsLong = Convert.ToInt64(_configField.GetValue());

        Dropdown.Clear();

        var index = 0;
        foreach (var valueUntyped in Enum.GetValues(configField.AssociatedType))
        {
            var value = Convert.ToInt64(valueUntyped);
            var enumName = Enum.GetName(configField.AssociatedType, value)!;
            _valueToIndex[value] = index;
            _indexToValue[index] = value;
            Dropdown.AddItem(enumName + $" ({value.ToString()})");
            index++;
        }

        Dropdown.Select(_valueToIndex[currentValueAsLong]);
    }

    public override void _EnterTree()
    {
        Dropdown.ItemSelected += ItemSelected;
    }

    public override void _ExitTree()
    {
        Dropdown.ItemSelected -= ItemSelected;
    }

    private void ItemSelected(long indexAsLong)
    {
        if (_configField == null)
        {
            LocalClient.Error("Enum Editor was not initialized");
            return;
        }

        var value = _indexToValue[(int)indexAsLong];

        var valueAsEnum = Enum.ToObject(_configField.AssociatedType, value);

        _configField.SetValue(valueAsEnum);
    }
}