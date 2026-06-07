using Godot;

namespace Sokomatic;

public class GlyphForeground
{
    private readonly Control _foreground;
    private readonly Label _label;
    private readonly TextureRect _textureRect;

    public GlyphForeground(Control foreground, Label label, TextureRect textureRect)
    {
        _foreground = foreground;
        _label = label;
        _textureRect = textureRect;
    }

    public void ShowNothing()
    {
        _label.Visible = false;
        _textureRect.Visible = false;
    }

    public void ShowGlyph(char character)
    {
        _label.Visible = true;
        _label.Text = $"{character}";

        _textureRect.Visible = false;
    }

    public void ShowImage(Texture2D? texture)
    {
        _textureRect.Visible = true;
        _textureRect.Texture = texture;

        _label.Visible = false;
    }

    public void SetColor(Color color)
    {
        _foreground.Modulate = color;
    }
}