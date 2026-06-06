using ExTween;
using Godot;

namespace SecretPlanGodot.Tweenables;

public class TweenableColor : Tweenable<Color>
{
    public TweenableColor() : base(new Color())
    {
    }
    
    public TweenableColor(Color initializedValue) : base(initializedValue)
    {
    }

    public TweenableColor(Getter getter, Setter setter) : base(getter, setter)
    {
    }

    public override Color Lerp(Color startingValue, Color targetValue, float percent)
    {
        return startingValue.Lerp(targetValue, percent);
    }
}