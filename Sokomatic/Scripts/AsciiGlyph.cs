using Godot;
using SecretPlanGodot.Core;
using SokoGame.Animation;

namespace Sokomatic;

public partial class AsciiGlyph : Control
{
    private readonly CachedNode<ColorRect> _backgroundColorRect = new("BackgroundColorRect");
    private readonly CachedNode<Control> _foregroundRoot = new("Foreground");
    private readonly CachedNode<Label> _label = new("Foreground/Label");
    private readonly ParentCore _parentCore = new();
    private readonly CachedNode<TextureRect> _textureRect = new("Foreground/TextureRect");
    private EntityAnimationState? _animationState;
    private Color _backgroundColor;
    private CoreState CoreState => _parentCore.State(this);

    public GlyphForeground Foreground { get; private set; } = null!;

    public Color BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            _backgroundColorRect.Get(this).Color = value;
            _backgroundColor = value;
        }
    }

    private Control ForegroundRoot => _foregroundRoot.Get(this);

    public override void _EnterTree()
    {
        Resized += OnControlResize;
        Foreground = new GlyphForeground(_foregroundRoot.Get(this), _label.Get(this), _textureRect.Get(this));

        Foreground.ShowNothing();
        BackgroundColor = Colors.Transparent;
    }

    private void OnControlResize()
    {
        _label.Get(this).AddThemeFontSizeOverride("font_size", (int)Size.Y);
    }

    public void SetAnimationState(EntityAnimationState animationState)
    {
        _animationState = animationState;
    }

    public override void _Process(double delta)
    {
        if (_animationState != null)
        {
            var positionOffsetPercent = _animationState.PositionOffsetPercent +
                                        _animationState.TweenablePositionOffsetPercent.Value;
            ForegroundRoot.Position = Size * positionOffsetPercent;
            ForegroundRoot.RotationDegrees = _animationState.AngleDegrees + _animationState.TweenableAngleDegrees.Value;
            var scale = _animationState.Scale * _animationState.TweenableScale.Value;
            ForegroundRoot.Scale = new Vector2(scale, scale);
            ForegroundRoot.Modulate = _animationState.PrimaryColor.Lerp(_animationState.SecondaryColor,
                _animationState.SecondaryColorPercent + _animationState.TweenableSecondaryColorPercent);
        }
    }
}