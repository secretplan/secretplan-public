using System;

namespace FriendShapedPlugin.ConfigEditor;

public readonly record struct SearchResult(bool Exists, string String, Action Choose, object? Value);