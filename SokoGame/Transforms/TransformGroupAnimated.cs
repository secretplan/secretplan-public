namespace SokoGame.Transforms;

public class TransformGroupAnimated : TransformGroup
{
    private readonly TransformAnimationType _animationType;

    public TransformGroupAnimated(TransformAnimationType animationType)
    {
        _animationType = animationType;
    }
}