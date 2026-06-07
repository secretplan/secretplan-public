using ExTween;
using Godot;
using SecretPlanGodot.Core;
using SokoCore;
using SokoGame.Animation;
using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct TransformNudge(EntityId EntityId, CardinalDirection NudgeDirection)
    : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        // no-op, but has animation
        return frame;
    }

    public void BuildBeforeAnimation(MultiplexTween tween, EntityViewTable table)
    {
        // for some strange reason this will weirdly tangle with other animations. i have no idea why 
        
        // var offset = Offset.FromDirection(NudgeDirection);
        // var directionOffsetAsVector = new Vector2(offset.X, offset.Y);
        // var entity = table.GetEntity(EntityId);
        // tween
        //     .Add(new SequenceTween()
        //         .Add(entity.TweenablePositionOffsetPercent.TweenTo(directionOffsetAsVector * 0.20f, 0.1f, Ease.QuadFastSlow))
        //         .Add(entity.TweenablePositionOffsetPercent.TweenTo(Vector2.Zero, 0.1f, Ease.QuadSlowFast))
        //     );
    }

    public void BuildAfterAnimation(MultiplexTween tween, EntityViewTable table)
    {
        var offset = Offset.FromDirection(NudgeDirection);
        var directionOffsetAsVector = new Vector2(offset.X, offset.Y);
        var entity = table.GetEntity(EntityId);
        tween
            .Add(new SequenceTween()
                .Add(entity.TweenablePositionOffsetPercent.TweenTo(directionOffsetAsVector * 0.20f, 0.1f, Ease.QuadFastSlow))
                .Add(entity.TweenablePositionOffsetPercent.TweenTo(Vector2.Zero, 0.1f, Ease.QuadSlowFast))
            );
    }

    public override string ToString()
    {
        return $"DO_NUDGE {EntityId} {NudgeDirection}";
    }
}