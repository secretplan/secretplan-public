using System;
using Godot;

namespace FriendShapedPlugin.ConfigEditor;

public partial class SearchResultButton : Button
{
    private Action? _action;
    public object? Value { get; private set; }

    public void Initialize(SearchResult searchResult)
    {
        _action = searchResult.Choose;
        Value = searchResult.Value;
    }

    public override void _ExitTree()
    {
        _action = null;
    }

    public override void _Pressed()
    {
        _action?.Invoke();
    }
}