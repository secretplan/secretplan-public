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
            PushingStrength = 5,
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
            RequiredStrengthToPush = 5,
            Phase = Phase.Solid,
            Density = Density.FloatsInLiquid,
            Graphic = EntityGraphic.CreateCharacter('O', 10)
        };
    }

    public static Entity Wall(GridPosition gridPosition)
    {
        return new Entity
        {
            Phase = Phase.Solid,
            Position = gridPosition,
            Graphic = EntityGraphic.CreateCharacter('#', 0)
        };
    }

    public static Entity GlassLightCrate(GridPosition gridPosition)
    {
        return new Entity
        {
            Position = gridPosition,
            RequiredStrengthToPush = 1,
            Phase = Phase.Solid,
            
            // Strong enough to push other glasses, but not strong enough to push crates
            PushingStrength = 2,
            Graphic = EntityGraphic.CreateCharacter('o', 10)
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
    
    public static Entity Pit(GridPosition gridPosition)
    {
        return new Entity
        {
            Position = gridPosition,
            Phase = Phase.Air,
            ReplacesFloor = true,
            Depth = WorldDepth.Floor,
            Graphic = EntityGraphic.CreateCharacter('~', 1)
        };
    }

    public static Entity BrittleFloor(GridPosition gridPosition)
    {
        return new Entity
        {
            Position = gridPosition,
            Phase = Phase.Solid,
            Depth = WorldDepth.Floor,
            Density = Density.FloatsInAir,
            Graphic = EntityGraphic.CreateCharacter(';', 1)
        };
    }
}