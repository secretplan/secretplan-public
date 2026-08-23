using System;
using BirdGame.Core;
using Godot;
using SecretPlan.Generated;
using SecretPlanCore.Core;
using SecretPlanGodot.ConfigEditor;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class ArrayEditor : FieldEditor
{
    private readonly CachedNode<Button> _addNewElementButton = new("AddButton");
    private readonly CachedNode<Control> _content = new("ArrayContent");
    private ConfigField? _arrayField;

    private Button AddNewElementButton => _addNewElementButton.Get(this);

    private Control Content => _content.Get(this);

    public override void Initialize(ConfigField field)
    {
        _arrayField = field;
        AddNewElementButton.Disabled = AssociatedTypeIsHiddenConfig();
        BuildDisplaysForAllElements();
    }

    private bool AssociatedTypeIsHiddenConfig()
    {
        if (_arrayField == null)
        {
            return false;
        }
        
        // This MIGHT be an array of configs, but it also might not be, so a null result here is reasonable
        var maybeConfigType = ConfigEnumTypeChecker.GetConfigTypeFromEnumType(_arrayField.AssociatedType);
        
        // If this is an array of configs, disable the button if that config type is hidden
        return maybeConfigType != null && FilteredConfigServer.IsTypeHidden(maybeConfigType);
    }

    private void BuildDisplaysForAllElements()
    {
        Content.QueueFreeAllChildren();
        
        if (_arrayField == null)
        {
            LocalClient.Error("Array Editor was not initialized!");
            return;
        }
        
        if (_arrayField.GetValue() is Array array)
        {
            for (var i = 0; i < array.Length; i++)
            {
                AddElementForIndex(i);
                _arrayField.CreateConfigFieldOfArrayElement(i);
            }
        }
    }

    public void SwapValues(int indexA, int indexB)
    {
        if (_arrayField == null)
        {
            return;
        }
        
        if (_arrayField.GetValue() is Array oldArray)
        {
            if (!oldArray.IsValidIndex(indexA) || !oldArray.IsValidIndex(indexB))
            {
                return;
            }
            
            var valueA = oldArray.GetValue(indexA);
            var valueB = oldArray.GetValue(indexB);

            var length = oldArray.Length;
            var newArray = Array.CreateInstance(_arrayField.AssociatedType, length);

            for (int i = 0; i < oldArray.Length; i++)
            {
                var oldValue = oldArray.GetValue(i);

                if (i == indexA)
                {
                    oldValue = valueB;
                }

                if (i == indexB)
                {
                    oldValue = valueA;
                }
                
                newArray.SetValue(oldValue, i);
            }

            _arrayField.SetValue(newArray);
            
            BuildDisplaysForAllElements();
        }
    }
    
    public void DeleteElementAtIndex(int indexToDelete)
    {
        if (_arrayField == null)
        {
            return;
        }
        
        if (_arrayField.GetValue() is Array oldArray)
        {
            if (!oldArray.IsValidIndex(indexToDelete))
            {
                return;
            }
            
            var newLength = oldArray.Length - 1;
            var newArray = Array.CreateInstance(_arrayField.AssociatedType, newLength);

            var offset = 0;
            for (int oldArrayIndex = 0; oldArrayIndex < oldArray.Length; oldArrayIndex++)
            {
                if (oldArrayIndex == indexToDelete)
                {
                    offset = -1;
                    continue;
                }
                newArray.SetValue(oldArray.GetValue(oldArrayIndex), oldArrayIndex+offset);
            }

            _arrayField.SetValue(newArray);
            
            BuildDisplaysForAllElements();
        }
    }

    public override void _EnterTree()
    {
        _addNewElementButton.Get(this).Pressed += OnNewButtonPressed;
    }

    public override void _ExitTree()
    {
        AddNewElementButton.Pressed -= OnNewButtonPressed;
    }

    private void OnNewButtonPressed()
    {
        if (_arrayField != null && _arrayField.GetValue() is Array array)
        {
            AddElementForIndex(array.Length);
        }
    }

    public FieldEditor? AddElementForIndex(int index)
    {
        if (_arrayField == null)
        {
            LocalClient.Error("ArrayEditor tried to create a new element but it was never initialized");
            return null;
        }
        
        var elementField = _arrayField.CreateConfigFieldOfArrayElement(index);
        var elementEditor = ConfigEditor.CreateEditorForConfigField(elementField);

        elementField.SetValue(elementField.GetValue()!);

        var elementWrapper =
            GodotUtilities.InstantiateScene<ArrayElementDisplay>("res://FriendShapedPlugin/ConfigEditor/Scenes/ArrayElement.tscn");

        elementWrapper.Controls.Setup(this, index, AssociatedTypeIsHiddenConfig());
        elementWrapper.SetEditor(elementEditor);
        Content.AddChild(elementWrapper);
        
        return elementEditor;
    }

    private int GetArrayLength()
    {
        if (_arrayField?.GetValue() is Array array)
        {
            return array.Length;
        }

        return 0;
    }
}