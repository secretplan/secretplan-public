using Godot;
using SecretPlanGodot.Core;

namespace SokoGodot;

public partial class AsciiGlyph : Control
{
    private readonly CachedNode<Control> _foregroundRoot = new("Foreground");
    private readonly CachedNode<Label> _label = new("Foreground/Label");
    private readonly CachedNode<TextureRect> _textureRect = new("Foreground/TextureRect");
    private Color _backgroundColor;
    private readonly CachedNode<ColorRect> _backgroundColorRect = new("BackgroundColorRect");

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

    public override void _EnterTree()
    {
        Resized += OnResize;
        Foreground = new GlyphForeground(_foregroundRoot.Get(this), _label.Get(this), _textureRect.Get(this));

        Foreground.ShowNothing();
        BackgroundColor = Colors.Transparent;
    }

    private void OnResize()
    {
        _label.Get(this).AddThemeFontSizeOverride("font_size", (int)Size.Y);
    }
}