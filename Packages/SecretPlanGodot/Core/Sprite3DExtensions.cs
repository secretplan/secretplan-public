using Godot;

namespace SecretPlanGodot.Core;

public static class Sprite3DExtensions
{
    public static int FrameCount(this Sprite3D sprite3D)
    {
        return sprite3D.Hframes * sprite3D.Vframes;
    }

    public static bool IsValidFrameIndex(this Sprite3D sprite3D, int index)
    {
        return index >= 0 && index < sprite3D.FrameCount();
    }
}