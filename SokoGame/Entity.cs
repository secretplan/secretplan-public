using SokoCore;

namespace SokoGame;

public readonly record struct Entity(bool IsActive, GridPosition? Position, CardinalDirection? MoveIntent);