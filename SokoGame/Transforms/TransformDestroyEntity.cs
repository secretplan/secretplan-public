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

    public override string ToString()
    {
        return $"DESTROY {EntityId}";
    }
}