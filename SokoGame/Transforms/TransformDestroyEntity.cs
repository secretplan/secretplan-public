using ExTween;
using SokoGame.Animation;
using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct TransformDestroyEntity(EntityId EntityId) : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        var entity = frame.GetEntity(EntityId);
        frame.SetEntity(EntityId, entity with { IsActive = false });
        return frame;
    }

    public void BuildBeforeAnimation(MultiplexTween tween, EntityViewTable table)
    {
    }

    public void BuildAfterAnimation(MultiplexTween tween, EntityViewTable table)
    {
    }

    public override string ToString()
    {
        return $"DESTROY {EntityId}";
    }
}