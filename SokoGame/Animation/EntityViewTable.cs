using SecretPlanGodot.Core;
using SokoGame.World;

namespace SokoGame.Animation;

public class EntityViewTable
{
    private readonly Dictionary<EntityId, EntityAnimationState> _animationTable = new();

    public IEnumerable<EntityAnimationState> Values()
    {
        return _animationTable.Values;
    }

    public EntityAnimationState GetEntity(EntityId id)
    {
        if (!_animationTable.ContainsKey(id))
        {
            LocalClient.Print($"Added animation state: {id}");
            _animationTable[id] = new EntityAnimationState();
        }

        return _animationTable[id];
    }
}