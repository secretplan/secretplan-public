using SokoCore;
using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct TransformStepOff(GridPosition Position) : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        foreach (var entityWithId in frame.AllActiveEntitiesWithIdsAtPosition(Position))
        {
            var entity = entityWithId.Entity;
            if (entity.BecomesHeavyWhenSteppedOff)
            {
                frame.SetEntity(entityWithId.Id, entity with { Density = Density.SinksInLiquid });
            }
        }

        return frame;
    }

    public override string ToString()
    {
        return $"STEP_OFF {Position}";
    }
}