using Godot;

namespace FriendShapedPlugin.ConfigEditor;

/// <summary>
///     Opposite of OnlyScrollOnShift
/// </summary>
public partial class IgnoreScrollOnShift : ScrollContainer
{
    private MouseFilterEnum _defaultFilter;
    private MouseFilterEnum _scrollbarFilterH;
    private MouseFilterEnum _scrollbarFilterV;

    public override void _Ready()
    {
        _defaultFilter = MouseFilter;
        _scrollbarFilterV = GetVScrollBar().MouseFilter;
        _scrollbarFilterH = GetHScrollBar().MouseFilter;
    }

    public override void _Process(double delta)
    {
        if (Input.IsKeyPressed(Key.Shift))
        {
            MouseFilter = MouseFilterEnum.Ignore;
            GetHScrollBar().MouseFilter = MouseFilterEnum.Ignore;
            GetVScrollBar().MouseFilter = MouseFilterEnum.Ignore;
        }
        else
        {
            MouseFilter = _defaultFilter;
            GetHScrollBar().MouseFilter = _scrollbarFilterH;
            GetVScrollBar().MouseFilter = _scrollbarFilterV;
        }
    }
}