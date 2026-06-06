using Godot;

namespace SecretPlanGodot.Extensions;

public static class Rect2Extensions
{
    public static Rect2 Lerp(this Rect2 self, Rect2 target, float percent)
    {
        return new Rect2(
            self.Position.Lerp(target.Position, percent),
            self.Size.Lerp(target.Size, percent)
        );
    } 
}