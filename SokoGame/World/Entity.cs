using SokoCore;

namespace SokoGame.World;

public readonly record struct Entity(
    bool IsActive,
    GridPosition? Position,
    CardinalDirection? FacingDirection,
    CardinalDirection? MoveIntent,
    Phase Phase,
    bool IsPlayerControlled,
    bool IsCameraOwner,
    bool AvoidsFalling,
    int? PushingStrength,
    int? RequiredStrengthToPush,
    bool ReplacesFloor,
    WorldDepth Depth,
    Density Density,
    EntityGraphic Graphic,
    bool BecomesHeavyWhenSteppedOff);