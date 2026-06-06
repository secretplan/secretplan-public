using System.Collections;
using SokoCore;

namespace SokoGame.World;

public class MoveResult
{
    private readonly List<EntityIdAndDirection> _entitiesToMove = new();
    private readonly HashSet<EntityId> _entitiesToNudge = new();

    public bool IsBlocked { get; private set; }

    public void AddNudge(EntityId entityId)
    {
        _entitiesToNudge.Add(entityId);
    }

    public void AddCascadingMoveIntent(EntityId entityId, CardinalDirection direction)
    {
        _entitiesToMove.Add(new EntityIdAndDirection(entityId, direction));
    }

    public void Block()
    {
        IsBlocked = true;
    }

    public IEnumerable<EntityIdAndDirection> CascadingMoveIntents()
    {
        return _entitiesToMove;
    }

    public IEnumerable<EntityId> NudgedEntities()
    {
        return _entitiesToNudge;
    }
}