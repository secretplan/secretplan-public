using SokoCore;

namespace SokoGame.World;

public readonly record struct Entity(bool IsActive, GridPosition? Position, CardinalDirection? MoveIntent);