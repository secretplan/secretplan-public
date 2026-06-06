using SokoCore;

namespace SokoGame.World;

public static class EntityTemplate
{
    public static Entity Player(GridPosition spawnPosition,
        CardinalDirection startingFacingDirection = CardinalDirection.Down)
    {
        return new Entity
        {
            Position = spawnPosition,
            FacingDirection = startingFacingDirection,
            Phase = Phase.Solid,
            IsPlayerControlled = true,
            PushingStrength = 2,
            IsCameraOwner = true,
            AvoidsFalling = true,
            Graphic = EntityGraphic.CreateCharacter('@', 20)
        };
    }

    public static Entity Crate(GridPosition gridPosition)
    {
        return new Entity
        {
            Position = gridPosition,
            RequiredStrengthToPush = 2,
            Phase = Phase.Solid,
            Density = Density.FloatsInLiquid,
            Graphic = EntityGraphic.CreateCharacter('H', 10)
        };
    }

    public static Entity Wall(GridPosition gridPosition)
    {
        return new Entity
        {
            Phase = Phase.Solid,
            Position = gridPosition,
            Graphic = EntityGraphic.CreateCharacter('%', 0)
        };
    }

    public static Entity GlassLightCrate(GridPosition gridPosition)
    {
        return new Entity
        {
            Position = gridPosition,
            RequiredStrengthToPush = 1,
            Phase = Phase.Solid,
            PushingStrength = 1,
            Graphic = EntityGraphic.CreateCharacter('G', 10)
        };
    }

    public static Entity Water(GridPosition gridPosition)
    {
        return new Entity
        {
            Position = gridPosition,
            Phase = Phase.Liquid,
            ReplacesFloor = true,
            Depth = WorldDepth.Floor,
            Graphic = EntityGraphic.CreateCharacter('~', 1)
        };
    }
}