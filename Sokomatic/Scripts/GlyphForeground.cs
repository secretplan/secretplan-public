using Godot;

namespace SokoGodot;

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
    
    public void ShowGlyph(char character, Color color)
    {
        _label.Visible = true;
        _label.Text = $"{character}";
        _foreground.Modulate = color;
        
        _textureRect.Visible = false;
    }

    public void ShowImage(Texture2D? texture, Color color)
    {
        _textureRect.Visible = true;
        _textureRect.Texture = texture;
        _foreground.Modulate = color;
        
        _label.Visible = false;
    }
}