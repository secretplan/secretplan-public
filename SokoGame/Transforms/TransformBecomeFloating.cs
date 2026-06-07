using ExTween;
using SokoGame.Animation;
using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct TransformBecomeFloating(EntityId EntityId) : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        var entity = frame.GetEntity(EntityId);
        frame.SetEntity(EntityId,
            entity with { Graphic = entity.Graphic with { Animation = EntityContinuousAnimation.Submerged } });
        return frame;
    }

    public void BuildBeforeAnimation(MultiplexTween tween, EntityViewTable table)
    {
    }

    public void BuildAfterAnimation(MultiplexTween tween, EntityViewTable table)
    {
        tween.Add(table.GetEntity(EntityId).TweenableScale.TweenTo(0.75f, 0.15f, Ease.QuadFastSlow));
    }

    public override string ToString()
    {
        return $"FLOAT {EntityId}";
    }
}