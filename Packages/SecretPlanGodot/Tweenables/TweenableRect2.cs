using ExTween;
using Godot;
using SecretPlanGodot.Extensions;

namespace SecretPlanGodot.Tweenables;

public class TweenableRect2 : Tweenable<Rect2>
{
    public TweenableRect2() : base(new Rect2())
    {
    }

    public TweenableRect2(Rect2 initializedValue) : base(initializedValue)
    {
    }

    public TweenableRect2(Getter getter, Setter setter) : base(getter, setter)
    {
    }

    public override Rect2 Lerp(Rect2 startingValue, Rect2 targetValue, float percent)
    {
        return startingValue.Lerp(targetValue, percent);
    }
}