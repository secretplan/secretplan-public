using ExTween;
using SokoCore;
using SokoGame.Animation;
using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct TransformChangeDepth(EntityId EntityId, WorldDepth TargetDepth) : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        var entity = frame.GetEntity(EntityId);
        frame.SetEntity(EntityId, entity with { Depth = TargetDepth });
        return frame;
    }

    public void BuildAnimation(MultiplexTween tween, EntityViewTable table)
    {
    }

    public override string ToString()
    {
        return $"CHANGE_DEPTH {EntityId} {TargetDepth}";
    }
}