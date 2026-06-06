using ExTween;
using Godot;

namespace SecretPlanGodot.Tweenables;

public class TweenableQuaternion : Tweenable<Quaternion>
{
    public TweenableQuaternion(Quaternion initializedValue) : base(initializedValue)
    {
    }

    public TweenableQuaternion(Getter getter, Setter setter) : base(getter, setter)
    {
    }

    public TweenableQuaternion() : base(Quaternion.Identity)
    {
    }

    public override Quaternion Lerp(Quaternion startingValue, Quaternion targetValue, float percent)
    {
        if (!targetValue.IsNormalized())
        {
            targetValue = targetValue.Normalized();
        }

        if (!startingValue.IsNormalized())
        {
            startingValue = startingValue.Normalized();
        }

        return startingValue.Slerp(targetValue, percent);
    }
}