using ExTween;
using Godot;
using SokoCore;
using SokoGame.Animation;
using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct TransformMoveEntityInCardinalDirection(EntityId EntityId, CardinalDirection Direction)
    : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        var entity = frame.GetEntity(EntityId);
        frame.SetEntity(EntityId, entity with { Position = entity.Position + Direction });
        return frame;
    }

    public void BuildAnimation(MultiplexTween tween, EntityViewTable table)
    {
        var offset = Offset.FromDirection(Direction);
        var directionOffsetAsVector = new Vector2(offset.X, offset.Y);
        var entity = table.GetEntity(EntityId);
        tween
            .Add(new SequenceTween()
                .Add(entity.TweenablePositionOffsetPercent.CallbackSetTo(directionOffsetAsVector * 0.15f))
                .Add(entity.TweenablePositionOffsetPercent.TweenTo(Vector2.Zero, 0.15f, Ease.QuadFastSlow))
            )
            ;
    }

    public override string ToString()
    {
        return $"MOVE {EntityId} {Direction}";
    }
}