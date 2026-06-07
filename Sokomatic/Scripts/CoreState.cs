using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;

namespace Sokomatic;

public class CoreState
{
    private readonly Dictionary<string, string?> _colorTable = new();

    public CoreState()
    {
        var readColorTable = JsonConvert.DeserializeObject<Dictionary<string, string>>(
            GameConstants.ReadTextResourceFile("res://Art/Colors.json"));

        if (readColorTable == null)
        {
            throw new Exception("Failed to read Colors.json");
        }

        foreach (var (key, value) in readColorTable)
        {
            _colorTable.Add(key, value);
        }
    }

    public Color ReadColor(string? colorNameOrHex)
    {
        if (colorNameOrHex == null)
        {
            return Colors.White;
        }

        return Color.FromHtml(_colorTable.GetValueOrDefault(colorNameOrHex, colorNameOrHex));
    }
}