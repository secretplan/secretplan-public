using Godot;
using SecretPlanGodot.Configuration;
using SecretPlanGodot.Core;

namespace FriendShapedPlugin.ConfigEditor;

public partial class TextureReferenceEditor : ResourceReferenceEditor<Texture2D>
{
    private readonly CachedNode<TextureRect> _textureRect = new("TextureRect");

    private TextureRect TextureRect => _textureRect.Get(this);

    protected override void Refresh()
    {
        var reference = GetCurrentValue();

        if (reference != null && reference.HasValue())
        {
            TextureRect.Visible = true;
            TextureRect.Texture = GodotUtilities.LoadResource<Texture2D>(reference.Path);
        }
        else
        {
            TextureRect.Visible = false;
            TextureRect.Texture = null;
        }
    }
}