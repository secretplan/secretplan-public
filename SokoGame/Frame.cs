using SokoGame.Transforms;

namespace SokoGame;

public class Frame
{
    private static uint _frameIdPool;
    private readonly uint _frameId;

    private readonly Dictionary<EntityId, Entity> _lookup = new();
    private EntityId _currentId;

    public Frame()
    {
        _frameId = _frameIdPool++;
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
        if (_lookup.ContainsKey(id))
        {
            return new EntityWithId(id, new Entity(), false);
        }

        return new EntityWithId(id, _lookup[id]);
    }

    public Entity GetEntity(EntityId id)
    {
        return GetEntityWithId(id).Entity;
    }

    public void SetEntity(EntityId entityId, Entity entity)
    {
        _lookup[entityId] = entity;
    }

    public Frame Clone()
    {
        var frame = new Frame();

        foreach (var (id, entity) in _lookup)
        {
            frame.SetEntity(id, entity);
        }

        return frame;
    }

    /// <summary>
    ///     Obtains the required transforms to get to a Resolved state after applying some initial transforms
    /// </summary>
    public AnimatedTransform GetTransformsToResolve(AnimatedTransform startingTransform)
    {
        var transforms = new AnimatedTransform(TransformAnimationType.InSequence);

        transforms.Add(startingTransform);
        var currentFrame = startingTransform.ApplyTo(Clone());
        
        // Figure out what we need to do next (if anything)
        var nextTransform = currentFrame.GetNextTransform();
        
        while (nextTransform.IsNoOp())
        {
            transforms.Add(nextTransform);
            currentFrame = nextTransform.ApplyTo(currentFrame.Clone());
            nextTransform = currentFrame.GetNextTransform();
        }

        return transforms;
    }

    /// <summary>
    ///     Gets the NEXT transform needed to get one step closer to resolved
    /// </summary>
    private ITransform GetNextTransform()
    {
        // The next transform will be the result of ONE of these functions, prioritized in this order.
        Func<AnimatedTransform>[] functions = [HandleAllMoveIntents, HandleSignalChanges, HandleSinks];

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

    private AnimatedTransform HandleSignalChanges()
    {
        var result = new AnimatedTransform(TransformAnimationType.AllAtOnce);

        return result;
    }

    private AnimatedTransform HandleSinks()
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