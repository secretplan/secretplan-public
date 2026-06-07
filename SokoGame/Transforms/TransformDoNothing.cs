using ExTween;
using SokoGame.Animation;
using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct TransformDoNothing : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        return frame;
    }

    public void BuildAnimation(MultiplexTween tween, EntityViewTable table)
    {
    }

    public override string ToString()
    {
        return "DO_NOTHING";
    }
}