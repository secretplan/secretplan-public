using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using SecretPlanCore.Core;
using SecretPlanGodot.Core;
using SokoCore;
using SokoGame.World;
using Sokomatic;
using Sokomatic.Aseprite;

namespace SokoGodot;

public partial class Core : Node
{
    private readonly CachedNode<AspectRatioContainer> _aspect = new("Aspect");
    private readonly CachedPackedScene<AsciiGlyph> _glyphPrefab = new("res://Scenes/Glyph.tscn");
    private readonly CachedPackedScene<Control> _linePrefab = new("res://Scenes/Line.tscn");
    private readonly Stack<Frame> _previousFrames = new();
    private readonly CachedNode<Control> _screen = new("Aspect/Lines");
    private readonly Dictionary<GridPosition, AsciiGlyph> _screenPositionToGlyph = new();
    private readonly SpriteLookup _spriteLookup = new();

    private Frame _currentFrame = new(new FrameIdSource());

    private Control Screen => _screen.Get(this);

    public override void _Ready()
    {
        InitializeScreen(16, 9);

        var atlasText = GameConstants.ReadTextResourceFile("res://Art/FullAtlas.json");
        var asepriteAtlas = JsonConvert.DeserializeObject<AsepriteSheetData>(atlasText);
        if (asepriteAtlas == null)
        {
            throw new Exception("Could not load atlas");
        }

        foreach (var (frameName, frame) in asepriteAtlas.Frames)
        {
            var key = frameName.RemoveFileExtension();

            if (key.StartsWith("ControllerButtons"))
            {
                _spriteLookup.Add(ImagePage.ControllerButtons, frame);
            }

            if (key.StartsWith("Entities"))
            {
                _spriteLookup.Add(ImagePage.Entities, frame);
            }

            if (key.StartsWith("Floors"))
            {
                _spriteLookup.Add(ImagePage.Floors, frame);
            }

            if (key.StartsWith("PopupFrame"))
            {
                _spriteLookup.Add(ImagePage.PopupFrame, frame);
            }

            if (key.StartsWith("Tools"))
            {
                _spriteLookup.Add(ImagePage.Tools, frame);
            }

            if (key.StartsWith("Utility"))
            {
                _spriteLookup.Add(ImagePage.Utility, frame);
            }

            if (key.StartsWith("Walls"))
            {
                _spriteLookup.Add(ImagePage.Walls, frame);
            }
        }

        _currentFrame.AddEntity(EntityTemplate.Player(new GridPosition(2, 2)));
        _currentFrame.AddEntity(EntityTemplate.Crate(new GridPosition(3, 2)));
        _currentFrame.AddEntity(EntityTemplate.GlassLightCrate(new GridPosition(3, 3)));
        _currentFrame.AddEntity(EntityTemplate.GlassLightCrate(new GridPosition(4, 3)));

        DrawCurrentFrame();
    }

    public override void _ExitTree()
    {
        _spriteLookup.Clear();
    }

    private void DrawCurrentFrame()
    {
        ClearScreen();

        foreach (var entityWithId in _currentFrame.AllActiveEntitiesWithIds())
        {
            var entity = entityWithId.Entity;
            if (entity.Position.HasValue)
            {
                PutGraphicAt(entity.Position.Value, entity.Graphic);
            }
        }
    }

    private void ClearScreen()
    {
        foreach (var (position, glyph) in _screenPositionToGlyph)
        {
            glyph.Foreground.ShowNothing();
        }
    }

    private void PutGraphicAt(GridPosition position, EntityGraphic graphic)
    {
        if (graphic.Mode == EntityGraphic.GraphicMode.Skip)
        {
            return;
        }

        var glyph = GetGlyphAt(position);

        if (glyph == null)
        {
            return;
        }

        if (graphic.Mode == EntityGraphic.GraphicMode.Character)
        {
            glyph.Foreground.ShowGlyph(graphic.Character);
        }

        if (graphic.Mode == EntityGraphic.GraphicMode.Sprite)
        {
            glyph.Foreground.ShowImage(_spriteLookup.Get(graphic.ImagePageIndex));
        }
    }

    public override void _Process(double delta)
    {
        DrawCurrentFrame();
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("ui_right"))
        {
            MovePlayerControlledEntities(CardinalDirection.Right);
        }

        if (inputEvent.IsActionPressed("ui_left"))
        {
            MovePlayerControlledEntities(CardinalDirection.Left);
        }

        if (inputEvent.IsActionPressed("ui_up"))
        {
            MovePlayerControlledEntities(CardinalDirection.Up);
        }

        if (inputEvent.IsActionPressed("ui_down"))
        {
            MovePlayerControlledEntities(CardinalDirection.Down);
        }

        if (inputEvent.IsActionPressed("undo"))
        {
            if (_previousFrames.TryPop(out var previousFrame))
            {
                _currentFrame = previousFrame;
                DrawCurrentFrame();
            }
        }
    }

    private void MovePlayerControlledEntities(CardinalDirection cardinalDirection)
    {
        foreach (var entityWithId in _currentFrame.AllActiveEntitiesWithIds())
        {
            if (entityWithId.Entity.IsPlayerControlled)
            {
                _currentFrame.SetEntity(entityWithId.Id, entityWithId.Entity with { MoveIntent = cardinalDirection });
            }
        }

        AdvanceFrame();
    }

    private void AdvanceFrame()
    {
        _previousFrames.Push(_currentFrame);
        _currentFrame = _currentFrame.CloneAndResolve();
    }

    public void InitializeScreen(int glyphsPerLine, int numberOfLines)
    {
        _aspect.Get(this).Ratio = (float)glyphsPerLine / numberOfLines;
        Screen.QueueFreeAllChildren();
        _screenPositionToGlyph.Clear();
        for (var y = 0; y < numberOfLines; y++)
        {
            var line = AddLine();
            for (var x = 0; x < glyphsPerLine; x++)
            {
                var glyph = _glyphPrefab.LoadAndInstantiate();
                _screenPositionToGlyph[new GridPosition(x, y)] = glyph;
                line.AddChild(glyph);
            }
        }
    }

    public void DrawOneLineString(GridPosition screenPosition, string text)
    {
        var writePosition = screenPosition;
        for (var i = 0; i < text.Length; i++)
        {
            GetGlyphAt(writePosition)?.Foreground.ShowGlyph(text[i]);
            writePosition += new Offset(1, 0);
        }
    }

    public AsciiGlyph? GetGlyphAt(GridPosition screenPosition)
    {
        return _screenPositionToGlyph.GetValueOrDefault(screenPosition);
    }

    private Control AddLine()
    {
        var line = _linePrefab.LoadAndInstantiate();
        Screen.AddChild(line);
        return line;
    }
}