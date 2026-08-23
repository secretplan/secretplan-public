using Godot;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class ArrayElementControls : Control
{
    private readonly CachedNode<Button> _deleteButton = new("DeleteButton");
    private readonly CachedNode<Button> _moveDownButton = new("DownButton");
    private readonly CachedNode<Button> _moveUpButton = new("UpButton");
    private ArrayEditor? _arrayEditor;
    private int _index;

    private Button DeleteButton => _deleteButton.Get(this);
    private Button MoveDownButton => _moveDownButton.Get(this);
    private Button MoveUpButton => _moveUpButton.Get(this);

    public void Setup(ArrayEditor arrayEditor, int index, bool shouldDisableButtons)
    {
        _arrayEditor = arrayEditor;
        _index = index;

        if (shouldDisableButtons)
        {
            DeleteButton.Disabled = true;
            MoveDownButton.Disabled = true;
            MoveUpButton.Disabled = true;
        }
    }

    public override void _EnterTree()
    {
        DeleteButton.Pressed += OnDeleteButtonPressed;
        MoveDownButton.Pressed += OnMoveDownButtonPressed;
        MoveUpButton.Pressed += OnMoveUpButtonPressed;
    }

    private void OnMoveUpButtonPressed()
    {
        SwapValueWithIndex(_index - 1);
    }

    private void SwapValueWithIndex(int otherIndex)
    {
        _arrayEditor?.SwapValues(_index, otherIndex);
    }

    private void OnMoveDownButtonPressed()
    {
        SwapValueWithIndex(_index + 1);
    }

    public override void _ExitTree()
    {
        DeleteButton.Pressed -= OnDeleteButtonPressed;
    }

    private void OnDeleteButtonPressed()
    {
        _arrayEditor?.DeleteElementAtIndex(_index);
    }
}