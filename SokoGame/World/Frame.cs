using SokoCore;
using SokoGame.Transforms;

namespace SokoGame.World;

public class Frame
{
    private readonly uint _frameId;
    private readonly FrameIdSource _frameIdSource;
    private EntityId _currentId;

    public Frame(FrameIdSource frameIdSource)
    {
        _frameIdSource = frameIdSource;
        _frameId = _frameIdSource.NextFrameId();
    }

    /// <summary>
    ///     This should be set to true if ANY "Set" method is called
    /// </summary>
    public bool HasChangedSinceClone { get; private set; }

    private Dictionary<EntityId, Entity> Lookup { get; init; } = new();

    public override string ToString()
    {
        return "Frame " + _frameId;
    }

    public EntityId AddEntity(Entity entity)
    {
        var id = _currentId++;
        Lookup[id] = entity with { IsActive = true };
        return id;
    }

    public EntityWithId GetEntityWithId(EntityId id)
    {
        if (!Lookup.TryGetValue(id, out var value))
        {
            return new EntityWithId(id, new Entity(), false);
        }

        return new EntityWithId(id, value);
    }

    public Entity GetEntity(EntityId id)
    {
        return GetEntityWithId(id).Entity;
    }

    public void SetEntity(EntityId entityId, Entity entity)
    {
        HasChangedSinceClone = true;
        Lookup[entityId] = entity;
    }

    /// <summary>
    ///     Makes a copy of this frame plus a transform
    /// </summary>
    public Frame CloneWithTransform(ITransform transform)
    {
        return transform.ApplyTo(Clone());
    }

    /// <summary>
    ///     Makes a copy of this frame plus a transform, plus any transforms needed to resolve it
    /// </summary>
    public Frame CloneWithTransformAndResolve(ITransform transform)
    {
        return CloneWithTransform(GetResolveTransform(transform));
    }

    /// <summary>
    ///     Makes a carbon-copy of this frame (with a different ID)
    /// </summary>
    public Frame Clone()
    {
        return new Frame(_frameIdSource)
        {
            Lookup = new Dictionary<EntityId, Entity>(Lookup)
        };
    }

    public AnimatedTransform GetResolveTransform()
    {
        return GetResolveTransform(new DoNothingTransform());
    }

    /// <summary>
    ///     Obtains a transform that will get us to a resolved state
    /// </summary>
    public AnimatedTransform GetResolveTransform(ITransform startingTransform)
    {
        if (startingTransform.IsNoOp())
        {
            Log($"Resolving from current state");
        }
        else
        {
            Log($"Resolving with starting transform: {startingTransform}");
        }

        var transforms = new AnimatedTransform(TransformAnimationType.InSequence);

        transforms.Add(startingTransform);
        var currentFrame = CloneWithTransform(startingTransform);

        // Figure out what we need to do next (if anything)
        var nextTransform = currentFrame.GetNextTransform();

        while (!nextTransform.IsNoOp())
        {
            currentFrame.Log($"Not resolved, applying {nextTransform}");
            transforms.Add(nextTransform);
            currentFrame = currentFrame.CloneWithTransform(nextTransform);
            nextTransform = currentFrame.GetNextTransform();
        }

        return transforms;
    }

    /// <summary>
    ///     Attempts to force the Frame into a resolved state, not guaranteed to be successful
    /// </summary>
    private void AttemptForceResolve()
    {
        foreach (var entityWithId in AllActiveEntitiesWithIds())
        {
            SetEntity(entityWithId, entityWithId.Entity with
            {
                MoveIntent = null
            });
        }
    }

    /// <summary>
    ///     This could be VERY slow depending on how many entities there are
    /// </summary>
    private bool IsSameAs(Frame otherFrame)
    {
        var ids = Lookup.Keys;
        return ids.Count == otherFrame.Lookup.Keys.Count && ids.All(id => GetEntity(id) == otherFrame.GetEntity(id));
    }

    public void Log(string message)
    {
        GlobalDebug.DebugLog($"{this}: {message}");
    }

    /// <summary>
    ///     Gets the NEXT transform needed to get one step closer to resolved
    /// </summary>
    private ITransform GetNextTransform()
    {
        // The next transform will be the result of ONE of these functions, prioritized in this order.
        Func<AnimatedTransform>[] functions =
            [HandleAllMoveIntents, HandleSignalChanges, HandleHeightChanges, HandleCollisions];

        foreach (var function in functions)
        {
            var result = function();
            if (!result.IsNoOp())
            {
                return result;
            }
        }

        return new DoNothingTransform();
    }

    private AnimatedTransform HandleCollisions()
    {
        var result = new AnimatedTransform(TransformAnimationType.AllAtOnce);

        return result;
    }

    private AnimatedTransform HandleSignalChanges()
    {
        var result = new AnimatedTransform(TransformAnimationType.AllAtOnce);

        return result;
    }

    private AnimatedTransform HandleHeightChanges()
    {
        var result = new AnimatedTransform(TransformAnimationType.AllAtOnce);

        return result;
    }

    private AnimatedTransform HandleAllMoveIntents()
    {
        var result = new AnimatedTransform(TransformAnimationType.AllAtOnce);

        foreach (var movingEntityWithId in AllActiveEntitiesWithIds())
        {
            var movingEntity = movingEntityWithId.Entity;
            if (!movingEntity.MoveIntent.HasValue || !movingEntity.Position.HasValue)
            {
                continue;
            }

            var move = CalculateMove(movingEntity, movingEntity.MoveIntent.Value);

            if (!move.IsBlocked)
            {
                // Move entity if the simulated move was not blocked
                result.Add(new MoveEntityInCardinalDirectionTransform(movingEntityWithId.Id,
                    movingEntity.MoveIntent.Value));
            }
            
            // Clear own move intent
            result.Add(new SetMoveIntentTransform(movingEntityWithId.Id, null));

            foreach (var movedEntity in move.CascadingMoveIntents())
            {
                result.Add(new SetMoveIntentTransform(movedEntity.Id, movedEntity.Direction));
            }

            foreach (var nudgedEntity in move.NudgedEntities())
            {
                result.Add(new SetNudgeIntentTransform(nudgedEntity, movingEntity.MoveIntent));
            }
        }

        return result;
    }

    private MoveResult CalculateMove(Entity movingEntity, CardinalDirection direction)
    {
        var moveResult = new MoveResult();
        
        var movingEntityDepth = movingEntity.Depth;
        var movingEntityPhase = movingEntity.Phase;
        
        if (movingEntityPhase == Phase.Immaterial)
        {
            return moveResult;
        }

        if (!movingEntity.Position.HasValue)
        {
            moveResult.Block();
            return moveResult;
        }

        foreach (var blockingEntityWithId in
                 AllActiveEntitiesWithIdsAtPosition(movingEntity.Position.Value + direction))
        {
            var blockingEntity = blockingEntityWithId.Entity;
            if (blockingEntity.Depth == movingEntityDepth && blockingEntity.Phase == movingEntityPhase)
            {
                if (!movingEntity.PushingStrength.HasValue)
                {
                    // We cannot push (so assume we're infinitely weak)
                    moveResult.Block();
                    break;
                }

                var myPushStrength = movingEntity.PushingStrength.Value;
                if (!blockingEntity.RequiredStrengthToPush.HasValue)
                {
                    // Blocker does not have a strength requirement (and is therefore infinitely heavy)
                    moveResult.Block();
                    break;
                }

                var pushRequirement = blockingEntity.RequiredStrengthToPush.Value;

                if (pushRequirement > myPushStrength)
                {
                    // Cannot push an object this heavy. Stop moving but nudge the object to show it can be moved.
                    moveResult.Block();
                    moveResult.AddNudge(blockingEntityWithId);
                }
                else if (pushRequirement == myPushStrength)
                {
                    // Can push exactly this weight. We stop moving but we also push the object.
                    moveResult.Block();
                    moveResult.AddCascadingMoveIntent(blockingEntityWithId, direction);
                }
                else if (pushRequirement < myPushStrength)
                {
                    // Can easily push this object, if it's able to move we'll move with it.
                    moveResult.AddCascadingMoveIntent(blockingEntityWithId, direction);
                    var subMove = CalculateMove(blockingEntity, direction);
                    if (subMove.IsBlocked)
                    {
                        moveResult.AddNudge(blockingEntityWithId);
                        moveResult.Block();
                    }
                }
            }
        }

        // todo: water -> if AvoidsFalling { return StopMoving; }

        return moveResult;
    }

    private IEnumerable<EntityWithId> AllActiveEntitiesWithIdsAtPosition(GridPosition targetPosition)
    {
        return AllActiveEntitiesWithIds().Where(a => a.Entity.Position == targetPosition);
    }

    private IEnumerable<EntityWithId> AllActiveEntitiesWithIds()
    {
        foreach (var (id, entity) in Lookup)
        {
            if (entity.IsActive)
            {
                yield return new EntityWithId(id, entity);
            }
        }
    }

    public bool IsResolved()
    {
        return GetResolveTransform().IsEmptyOrNoOp();
    }
}