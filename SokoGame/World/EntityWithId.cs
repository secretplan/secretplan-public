namespace SokoGame.World;

public readonly record struct EntityWithId(EntityId Id, Entity Entity, bool IsValid = true)
{
    public static implicit operator EntityId(EntityWithId entityWithId)
    {
        if (!entityWithId.IsValid)
        {
            throw new Exception("Invalid Entity!");
        }
        
        return entityWithId.Id;
    }

    public static implicit operator Entity(EntityWithId entityWithId)
    {
        if (!entityWithId.IsValid)
        {
            throw new Exception("Invalid Entity!");
        }
        
        return entityWithId.Entity;
    }
}