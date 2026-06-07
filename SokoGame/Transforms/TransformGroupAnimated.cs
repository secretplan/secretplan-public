namespace SokoGame.Transforms;

public class TransformGroupAnimated : TransformGroup
{
    public TransformAnimationType AnimationType { get; }

    public TransformGroupAnimated(TransformAnimationType animationType)
    {
        AnimationType = animationType;
    }

    protected override string ToStringPrefix()
    {
        return AnimationType.ToString();
    }
}