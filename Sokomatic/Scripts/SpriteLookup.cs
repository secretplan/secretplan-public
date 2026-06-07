using System.Collections.Generic;
using Godot;
using SecretPlanCore.Core;
using SecretPlanGodot.Core;
using SokoGame.World;
using Sokomatic.Aseprite;

namespace Sokomatic;

public class SpriteLookup
{
    private readonly CachedResource<Texture2D> _atlas = new("res://Art/FullAtlas.png");
    private readonly Dictionary<ImagePage, List<AtlasTexture>> _pages = new();

    public void Add(ImagePage page, AsepriteFrame asepriteFrame)
    {
        var atlasTexture = new AtlasTexture();
        atlasTexture.Atlas = _atlas.GetOrLoad();
        atlasTexture.FilterClip = true;
        atlasTexture.Region = new Rect2
        {
            Position = new Vector2I(asepriteFrame.Frame.X, asepriteFrame.Frame.Y),
            Size = new Vector2I(asepriteFrame.Frame.Width, asepriteFrame.Frame.Height)
        };

        _pages.TryAdd(page, new List<AtlasTexture>());
        _pages[page].Add(atlasTexture);
    }

    public void Clear()
    {
        foreach (var (key, value) in _pages)
        {
            foreach (var item in value)
            {
                item.Dispose();
            }
        }

        _pages.Clear();
    }

    public Texture2D? Get(ImagePageIndex pageIndex)
    {
        _pages.TryAdd(pageIndex.Page, new List<AtlasTexture>());
        var list = _pages[pageIndex.Page];

        if (list.IsValidIndex(pageIndex.Index))
        {
            return list[pageIndex.Index];
        }

        return null;
    }
}