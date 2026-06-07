using SokoCore;
using SokoGame.Transforms;

namespace SokoGame.World;

public static class Rules
{
    public static TransformGroupAnimated HandleAllMoveIntents(Frame frame)
    {
        var result = new TransformGroupAnimated(TransformAnimationType.NonBlocking);

        foreach (var (movingEntityId, movingEntity, _) in frame.AllActiveEntitiesWithIds())
        {
            if (movingEntity.NudgeIntent.HasValue)
            {
                // If we have a pending nudge, resolve that
                result.Add(new TransformSetNudgeIntent(movingEntityId, null));
                result.Add(new TransformNudge(movingEntityId, movingEntity.NudgeIntent.Value));
            }
            
            if (!movingEntity.MoveIntent.HasValue || !movingEntity.Position.HasValue)
            {
                continue;
            }

            var move = frame.CalculateMove(movingEntity, movingEntity.MoveIntent.Value);

            if (!move.IsBlocked)
            {
                result.Add(new TransformMoveCardinal(movingEntityId, movingEntity.MoveIntent.Value));
                result.Add(new TransformStepOff(movingEntity.Position.Value));
            }
            else
            {
                result.Add(new TransformSetNudgeIntent(movingEntityId, movingEntity.MoveIntent.Value));
            }

            result.Add(new TransformSetMoveIntent(movingEntityId, null));

            foreach (var cascadingMove in move.CascadingMoveIntents())
            {
                result.Add(new TransformSetMoveIntent(cascadingMove.Id, cascadingMove.Direction));
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
        var result = new TransformGroupAnimated(TransformAnimationType.Blocking);

        return result;
    }

    public static TransformGroupAnimated HandleSignalChanges(Frame frame)
    {
        var result = new TransformGroupAnimated(TransformAnimationType.NonBlocking);

        return result;
    }

    public static TransformGroupAnimated HandleHeightChanges(Frame frame)
    {
        var result = new TransformGroupAnimated(TransformAnimationType.Blocking);

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