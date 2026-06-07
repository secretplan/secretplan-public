using ExTween;

namespace SokoGame.Transforms;

public class TransformGroupAnimated : TransformGroup
{
    public TransformGroupAnimated(TransformAnimationType animationType)
    {
        AnimationType = animationType;
    }

    public TransformAnimationType AnimationType { get; }

    protected override string ToStringPrefix()
    {
        return AnimationType.ToString();
    }

    public IEnumerable<TransformGroupAnimated> AllAnimated()
    {
        foreach (var transform in All())
        {
            if (transform is not TransformGroupAnimated animated)
            {
                continue;
            }

            if (transform.IsNoOp())
            {
                continue;
            }

            yield return animated;
        }
    }

    public void AppendTween(SequenceTween tween, ref MultiplexTween currentMultiplex)
    {
        if (AnimationType == TransformAnimationType.Blocking)
        {
            tween.Add(currentMultiplex);
            currentMultiplex = new MultiplexTween();
        }
    }
}