namespace SokoGame.Transforms;

public class AnimatedTransform : TransformGroup
{
    private readonly TransformAnimationType _animationType;

    public AnimatedTransform(TransformAnimationType animationType)
    {
        _animationType = animationType;
    }
}