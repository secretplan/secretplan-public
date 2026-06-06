using SokoGame.Transforms;

namespace SokoGame.World;

public class Frame
{
    private readonly uint _frameId;
    private readonly FrameIdSource _frameIdSource;

    private readonly Dictionary<EntityId, Entity> _lookup = new();
    private EntityId _currentId;

    public Frame(FrameIdSource frameIdSource)
    {
        _frameIdSource = frameIdSource;
        _frameId = _frameIdSource.NextFrameId();
    }

    public override string ToString()
    {
        return "Frame " + _frameId;
    }

    public EntityId AddEntity(Entity entity)
    {
        var id = _currentId++;
        _lookup[id] = entity with { IsActive = true };
        return id;
    }

    public EntityWithId GetEntityWithId(EntityId id)
    {
        if (!_lookup.TryGetValue(id, out var value))
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
        _lookup[entityId] = entity;
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
        var frame = new Frame(_frameIdSource);

        foreach (var (id, entity) in _lookup)
        {
            frame.SetEntity(id, entity);
        }

        return frame;
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
        Log($"GetTransformsToResolve with starting transform: {startingTransform}");
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

    private void Log(string message)
    {
        Global.DebugLog($"{this}: {message}");
    }

    /// <summary>
    ///     Gets the NEXT transform needed to get one step closer to resolved
    /// </summary>
    private ITransform GetNextTransform()
    {
        // The next transform will be the result of ONE of these functions, prioritized in this order.
        Func<AnimatedTransform>[] functions = [HandleAllMoveIntents, HandleSignalChanges, HandleHeightChanges, HandleCollisions];

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

        foreach (var entityWithId in AllActiveEntitiesWithIds())
        {
            if (entityWithId.Entity.MoveIntent != null)
            {
                // todo: If entity cannot move here, do not let it
                result.Add(new MoveEntityInCardinalDirectionTransform(entityWithId.Id,
                    entityWithId.Entity.MoveIntent.Value));

                result.Add(new SetMoveIntentTransform(entityWithId.Id, null));
            }
        }

        return result;
    }

    private IEnumerable<EntityWithId> AllActiveEntitiesWithIds()
    {
        foreach (var (id, entity) in _lookup)
        {
            if (entity.IsActive)
            {
                yield return new EntityWithId(id, entity);
            }
        }
    }
}