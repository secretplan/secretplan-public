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

    public TransformGroupAnimated GetResolveTransform()
    {
        return GetResolveTransform(new TransformDoNothing());
    }

    /// <summary>
    ///     Obtains a transform that will get us to a resolved state
    /// </summary>
    public TransformGroupAnimated GetResolveTransform(ITransform startingTransform)
    {
        if (startingTransform.IsNoOp())
        {
            Log("Resolving from current state");
        }
        else
        {
            Log($"Resolving with starting transform: {startingTransform}");
        }

        var transforms = new TransformGroupAnimated(TransformAnimationType.InSequence);

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
        Func<Frame, TransformGroupAnimated>[] functions =
            [Rules.HandleAllMoveIntents, Rules.HandleSignalChanges, Rules.HandleHeightChanges, Rules.HandleCollisions];

        foreach (var function in functions)
        {
            var result = function(this);
            if (!result.IsNoOp())
            {
                return result;
            }
        }

        return new TransformDoNothing();
    }

    public IEnumerable<HoleDescription> AllHoles()
    {
        foreach (var gridPosition in AllGridPositions())
        {
            var hole = GetHoleAt(gridPosition);
            if (hole != null)
            {
                yield return hole;
            }
        }
    }

    public bool IsHoleAt(GridPosition gridPosition)
    {
        var hole = GetHoleAt(gridPosition);
        return hole != null && hole.IsValid;
    }

    private HoleDescription? GetHoleAt(GridPosition gridPosition)
    {
        var entities = AllActiveEntitiesWithIdsAtPosition(gridPosition).ToList();
        var holes = entities.Where(a => a.Entity.ReplacesFloor).ToHashSet();

        if (holes.Count == 0)
        {
            return null;
        }

        var fillers = entities.Where(a => a.Entity.Depth == WorldDepth.Floor && a.Entity.Phase == Phase.Solid).ToList();

        var filledHoles = new HashSet<EntityId>();
        foreach (var hole in holes)
        {
            // Water/Lava needs to be filled by something that floats in liquid
            if (hole.Entity.Phase == Phase.Liquid)
            {
                if (fillers.Any(a => a.Entity.Density <= Density.FloatsInLiquid))
                {
                    filledHoles.Add(hole);
                }
            }

            // Pit needs to be filled by something that floats in liquid
            if (hole.Entity.Phase == Phase.Air)
            {
                if (fillers.Any(a => a.Entity.Density <= Density.FloatsInAir))
                {
                    filledHoles.Add(hole);
                }
            }
        }

        holes.RemoveWhere(a => filledHoles.Contains(a.Id));

        if (holes.Count == 0)
        {
            // all holes filled, no hole here!
            return null;
        }

        return new HoleDescription(gridPosition, holes);
    }

    private IEnumerable<GridPosition> AllGridPositions()
    {
        return AllActiveEntitiesWithIds()
            .Select(a => a.Entity.Position)
            .Where(a => a.HasValue)
            .Select(position => position!.Value)
            .Distinct();
    }


    public MoveResult CalculateMove(Entity movingEntity, CardinalDirection direction)
    {
        var moveResult = new MoveResult();

        var movingEntityDepth = movingEntity.Depth;
        var movingEntityPhase = movingEntity.Phase;

        if (movingEntityPhase == Phase.Air)
        {
            return moveResult;
        }

        if (!movingEntity.Position.HasValue)
        {
            moveResult.Block();
            return moveResult;
        }

        var targetPosition = movingEntity.Position.Value + direction;
        foreach (var blockingEntityWithId in AllActiveEntitiesWithIdsAtPosition(targetPosition))
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
        
        if (movingEntity.AvoidsFalling && IsHoleAt(targetPosition))
        {
            moveResult.Block();
        }

        return moveResult;
    }

    public IEnumerable<EntityWithId> AllActiveEntitiesWithIdsAtPosition(GridPosition targetPosition)
    {
        return AllActiveEntitiesWithIds().Where(a => a.Entity.Position == targetPosition);
    }

    public IEnumerable<EntityWithId> AllActiveEntitiesWithIds()
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