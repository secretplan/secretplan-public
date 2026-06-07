using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct TransformBecomeFloating(EntityId EntityId) : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        var entity = frame.GetEntity(EntityId);
        frame.SetEntity(EntityId, entity with {Graphic = entity.Graphic with {Animation = EntityContinuousAnimation.Submerged}});
        return frame;
    }

    public override string ToString()
    {
        return $"FLOAT {EntityId}";
    }
}