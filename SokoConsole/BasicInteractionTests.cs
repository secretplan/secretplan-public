using SokoCore;
using SokoGame.Transforms;
using SokoGame.World;

namespace SokoConsole2;

public class BasicInteractionTests : BaseTests
{
    public void PlayerMovesInEmptyVoid()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));

        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        SimpleAssert.ShouldBe(StartingFrame.GetEntity(player).Position, GridPosition.Zero);
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(1, 0));
    }

    public void PlayerBlockedByWall()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        var wall = StartingFrame.AddEntity(EntityTemplate.Wall(new GridPosition(2, 0)));

        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));
        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(1, 0));
    }

    public void PlayerPushesCrate()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        var crate = StartingFrame.AddEntity(EntityTemplate.Crate(new GridPosition(1, 0)));

        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        // crate has moved, player has not
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(0, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(crate).Position, new GridPosition(2, 0));

        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        // player has moved into the space that crate has vacated
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(1, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(crate).Position, new GridPosition(2, 0));
    }

    public void PlayerPushesTwoAdjacentCrates()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        var crate1 = StartingFrame.AddEntity(EntityTemplate.Crate(new GridPosition(1, 0)));
        var crate2 = StartingFrame.AddEntity(EntityTemplate.Crate(new GridPosition(2, 0)));

        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        // Nobody moved
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(0, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(crate1).Position, new GridPosition(1, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(crate2).Position, new GridPosition(2, 0));
    }

    public void PlayerPushesGlass()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        var glass = StartingFrame.AddEntity(EntityTemplate.GlassLightCrate(new GridPosition(1, 0)));

        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        // glass has moved, so has player
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(1, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(glass).Position, new GridPosition(2, 0));

        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        // glass has moved again
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(2, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(glass).Position, new GridPosition(3, 0));
    }

    public void PlayerPushesMultipleGlass()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        var glass1 = StartingFrame.AddEntity(EntityTemplate.GlassLightCrate(new GridPosition(1, 0)));
        var glass2 = StartingFrame.AddEntity(EntityTemplate.GlassLightCrate(new GridPosition(2, 0)));

        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        // Everybody moved
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(1, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(glass1).Position, new GridPosition(2, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(glass2).Position, new GridPosition(3, 0));
    }

    public void PlayerPushesMultipleGlass_DifferentOrder()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        var glass2 = StartingFrame.AddEntity(EntityTemplate.GlassLightCrate(new GridPosition(2, 0)));
        var glass1 = StartingFrame.AddEntity(EntityTemplate.GlassLightCrate(new GridPosition(1, 0)));

        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        // Everybody moved
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(1, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(glass1).Position, new GridPosition(2, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(glass2).Position, new GridPosition(3, 0));
    }

    public void PlayerPushesCrateIntoWall()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        var crate = StartingFrame.AddEntity(EntityTemplate.Crate(new GridPosition(1, 0)));
        var wall = StartingFrame.AddEntity(EntityTemplate.Wall(new GridPosition(2, 0)));

        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        // Nobody moved
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(0, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(crate).Position, new GridPosition(1, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(wall).Position, new GridPosition(2, 0));
        SimpleAssert.ShouldBe(CurrentFrame.IsResolved(), true);
    }

    public void PlayerPushesGlassIntoWall()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        var glass = StartingFrame.AddEntity(EntityTemplate.GlassLightCrate(new GridPosition(1, 0)));
        var wall = StartingFrame.AddEntity(EntityTemplate.Wall(new GridPosition(2, 0)));

        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        // Nobody moved
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(0, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(glass).Position, new GridPosition(1, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(wall).Position, new GridPosition(2, 0));
        SimpleAssert.ShouldBe(CurrentFrame.IsResolved(), true);
    }

    public void PlayerBlockedByWater()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        var water = StartingFrame.AddEntity(EntityTemplate.Water(new GridPosition(1, 0)));

        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, GridPosition.Zero);
    }

    public void PushCrateIntoWaterAndWalkOnIt()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        var crate = StartingFrame.AddEntity(EntityTemplate.Crate(new GridPosition(1, 0)));
        var water = StartingFrame.AddEntity(EntityTemplate.Water(new GridPosition(2, 0)));

        // Push the crate into the water
        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        // Move to 1, 0
        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        // Move to 2, 0
        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(2, 0));
    }

    public void OneWayDoorByWayOfBrittleFloor()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        StartingFrame.AddEntity(EntityTemplate.BrittleFloor(new GridPosition(1, 0)));
        StartingFrame.AddEntity(EntityTemplate.Water(new GridPosition(1, 0)));
        
        // step on floor
        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));
        
        
        // player moved successfully, no hole yet!
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(1,0));
        SimpleAssert.ShouldBe(CurrentFrame.IsHoleAt(new GridPosition(1,0)), false);
        
        // step off floor, revealing water underneath
        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        // There is a hole now!
        SimpleAssert.ShouldBe(CurrentFrame.IsHoleAt(new GridPosition(1,0)), true);
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(2,0));
        
        // Attempt to move backwards
        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Left));
        
        // Player was blocked by water
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(2,0));
    }

    public void MultipleObjectsMoveToSameSpotOnSameFrame()
    {
    }

    private void PrintStatus()
    {
    }
}