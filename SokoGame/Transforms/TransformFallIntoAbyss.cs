using ExTween;
using Godot;
using SokoGame.Animation;
using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct TransformFallIntoAbyss(EntityId EntityId) : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        // no-op, but we will animate this
        return frame;
    }

    public void BuildBeforeAnimation(MultiplexTween tween, EntityViewTable table)
    {
        var entity = table.GetEntity(EntityId);
        tween
            .Add(entity.CallbackSetSecondaryColor(Colors.Transparent))
            .Add(entity.TweenableSecondaryColorPercent.TweenTo(1f, 0.5f, Ease.Linear))
            .Add(entity.TweenableScale.TweenTo(0f, 1f, Ease.QuadSlowFast))
            ;
    }

    public void BuildAfterAnimation(MultiplexTween tween, EntityViewTable table)
    {
        
    }

    public override string ToString()
    {
        return $"FALL_ABYSS {EntityId}";
    }
}