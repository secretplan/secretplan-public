using SokoCore;
using SokoGame.Transforms;

namespace SokoGame.World;

public static class Rules
{
    public static TransformGroupAnimated HandleAllMoveIntents(Frame frame)
    {
        var result = new TransformGroupAnimated(TransformAnimationType.AllAtOnce);

        foreach (var (entityId, movingEntity, _) in frame.AllActiveEntitiesWithIds())
        {
            if (!movingEntity.MoveIntent.HasValue || !movingEntity.Position.HasValue)
            {
                continue;
            }

            var move = frame.CalculateMove(movingEntity, movingEntity.MoveIntent.Value);

            if (!move.IsBlocked)
            {
                // Move entity if the simulated move was not blocked
                result.Add(new MoveEntityInCardinalDirectionTransform(entityId,
                    movingEntity.MoveIntent.Value));
            }

            // Clear own move intent
            result.Add(new TransformSetMoveIntent(entityId, null));

            foreach (var movedEntity in move.CascadingMoveIntents())
            {
                result.Add(new TransformSetMoveIntent(movedEntity.Id, movedEntity.Direction));
            }

            foreach (var nudgedEntity in move.NudgedEntities())
            {
                result.Add(new TransformSetNudgeIntent(nudgedEntity, movingEntity.MoveIntent));
            }
        }

        return result;
    }
    
    public static TransformGroupAnimated HandleCollisions(Frame frame)
    {
        var result = new TransformGroupAnimated(TransformAnimationType.AllAtOnce);

        return result;
    }

    public static TransformGroupAnimated HandleSignalChanges(Frame frame)
    {
        var result = new TransformGroupAnimated(TransformAnimationType.AllAtOnce);

        return result;
    }

    public static TransformGroupAnimated HandleHeightChanges(Frame frame)
    {
        var result = new TransformGroupAnimated(TransformAnimationType.AllAtOnce);

        foreach (var hole in frame.AllHoles())
        {
            foreach (var fillingEntityWithId in frame.AllActiveEntitiesWithIdsAtPosition(hole.Position))
            {
                var fillingEntity = fillingEntityWithId.Entity;
                var interaction = hole.GetHoleInteraction(fillingEntity);
                switch (interaction)
                {
                    case HoleInteraction.Fill:
                        result.Add(new TransformChangeDepth(fillingEntityWithId, WorldDepth.Floor));
                        result.Add(new TransformBecomeFloating(fillingEntityWithId));
                        break;
                    case HoleInteraction.Sink:
                        result.Add(new TransformFallIntoAbyss(fillingEntityWithId));
                        result.Add(new TransformDestroyEntity(fillingEntityWithId));
                        break;
                }
            }
        }

        return result;
    }
}