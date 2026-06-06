namespace SokoGame;

public readonly record struct EntityWithId(EntityId Id, Entity Entity, bool IsValid = true);