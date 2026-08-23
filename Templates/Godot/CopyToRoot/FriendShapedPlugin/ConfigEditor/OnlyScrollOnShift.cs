using Godot;

namespace FriendShapedPlugin.ConfigEditor;

/// <summary>
///     Sorta the opposite of IgnoreScrollOnShift, however it doesn't tell the scrollbars they can't scroll
/// </summary>
public partial class OnlyScrollOnShift : ScrollContainer
{
    private MouseFilterEnum _defaultFilter;

    public override void _Ready()
    {
        _defaultFilter = MouseFilter;
    }

    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Shift))
        {
            MouseFilter = _defaultFilter;
        }
        else
        {
            MouseFilter = MouseFilterEnum.Ignore;
        }
    }
}