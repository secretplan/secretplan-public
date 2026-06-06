using ExTween;
using Godot;

namespace SecretPlanGodot.Tweenables;

public class TweenableVector2 : Tweenable<Vector2>
{
    public TweenableVector2() : base(new Vector2())
    {
    }

    public TweenableVector2(Vector2 initializedValue) : base(initializedValue)
    {
    }

    public TweenableVector2(Getter getter, Setter setter) : base(getter, setter)
    {
    }

    public override Vector2 Lerp(Vector2 startingValue, Vector2 targetValue, float percent)
    {
        return startingValue.Lerp(targetValue, percent);
    }
}