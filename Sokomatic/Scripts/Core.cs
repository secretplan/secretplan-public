using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Newtonsoft.Json;
using SecretPlanCore.Core;
using SecretPlanGodot.Core;
using SokoCore;
using SokoGame.World;
using Sokomatic.Aseprite;

namespace Sokomatic;

public partial class Core : Node
{
    private readonly CachedNode<AspectRatioContainer> _aspect = new("Aspect");
    private readonly CachedPackedScene<AsciiGlyph> _glyphPrefab = new("res://Scenes/Glyph.tscn");
    private readonly CachedPackedScene<Control> _linePrefab = new("res://Scenes/Line.tscn");
    private readonly CachedNode<Control> _screen = new("Aspect/Lines");
    private readonly Dictionary<GridPosition, AsciiGlyph> _screenPositionToGlyph = new();
    private readonly SpriteLookup _spriteLookup = new();

    private GameSession _gameSession = new();
    public CoreState CoreState { get; } = new();

    private Control Screen => _screen.Get(this);

    public override void _Ready()
    {
        InitializeScreen(16, 9);

        var asepriteAtlas =
            JsonConvert.DeserializeObject<AsepriteSheetData>(
                GameConstants.ReadTextResourceFile("res://Art/FullAtlas.json"));
        if (asepriteAtlas == null)
        {
            throw new Exception("Could not load FullAtlas.json");
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

        HardReset();

        DrawCurrentFrame();
    }

    private void HardReset()
    {
        // Clear old game session
        _gameSession.FrameChanged -= DrawCurrentFrame;

        // Create new game session
        _gameSession = new GameSession();
        _gameSession.FrameChanged += DrawCurrentFrame;
    }

    public override void _ExitTree()
    {
        _spriteLookup.Clear();
    }

    private void DrawCurrentFrame()
    {
        ClearScreen();

        var allEntities = _gameSession.CurrentFrame.AllActiveEntitiesWithIds().ToList();
        allEntities.Sort((entityA, entityB) =>
            entityA.Entity.Graphic.LayerIndex.CompareTo(entityB.Entity.Graphic.LayerIndex));
        foreach (var entityWithId in allEntities)
        {
            var entity = entityWithId.Entity;
            if (entity.Position.HasValue)
            {
                var animationState = _gameSession.GetAnimationState(entityWithId.Id);
                animationState.CurrentAnimation = entity.Graphic.Animation;
                animationState.PrimaryColor = CoreState.ReadColor(entity.Graphic.Color);
                PutGraphicAt(entity.Position.Value, entity.Graphic, animationState);
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

    private void PutGraphicAt(GridPosition position, EntityGraphic graphic, EntityAnimationState animationState)
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

        glyph.Foreground.SetColor(CoreState.ReadColor(graphic.Color));
        glyph.SetAnimationState(animationState);
    }

    public override void _Process(double delta)
    {
        _gameSession.UpdateAnimationStates((float)delta);
    }

    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("move_right"))
        {
            _gameSession.HandleDirectionalInput(CardinalDirection.Right);
        }

        if (inputEvent.IsActionPressed("move_left"))
        {
            _gameSession.HandleDirectionalInput(CardinalDirection.Left);
        }

        if (inputEvent.IsActionPressed("move_up"))
        {
            _gameSession.HandleDirectionalInput(CardinalDirection.Up);
        }

        if (inputEvent.IsActionPressed("move_down"))
        {
            _gameSession.HandleDirectionalInput(CardinalDirection.Down);
        }

        if (inputEvent.IsActionPressed("undo"))
        {
            _gameSession.Undo();
        }

        if (inputEvent.IsActionPressed("hard-reset"))
        {
            HardReset();
        }

        if (inputEvent.IsActionPressed("reset"))
        {
            _gameSession.SoftReset();
        }

        if (inputEvent.IsActionPressed("primary_action"))
        {
            _gameSession.PrimaryAction();
        }

        if (inputEvent.IsActionPressed("secondary_action"))
        {
            _gameSession.SecondaryAction();
        }
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

    public void DrawOneLineString(GridPosition screenPosition, string text, Color color)
    {
        var writePosition = screenPosition;
        for (var i = 0; i < text.Length; i++)
        {
            GetGlyphAt(writePosition)?.Foreground.ShowGlyph(text[i]);
            GetGlyphAt(writePosition)?.Foreground.SetColor(color);
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