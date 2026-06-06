using SokoCore;
using SokoGame.Transforms;
using SokoGame.World;

namespace SokoConsole2;

public class BasicTests : BaseTests
{
    public void PlayerMovesInEmptyVoid()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));

        ApplyAndResolve(new SetMoveIntentTransform(player, CardinalDirection.Right));

        SimpleAssert.ShouldBe(StartingFrame.GetEntity(player).Position, GridPosition.Zero);
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(1, 0));
    }

    public void PlayerHitsWall()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        var wall = StartingFrame.AddEntity(EntityTemplate.Wall(new GridPosition(2, 0)));

        ApplyAndResolve(new SetMoveIntentTransform(player, CardinalDirection.Right));
        ApplyAndResolve(new SetMoveIntentTransform(player, CardinalDirection.Right));

        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(1, 0));
    }

    public void PlayerPushesCrate()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        var crate = StartingFrame.AddEntity(EntityTemplate.Crate(new GridPosition(1, 0)));

        ApplyAndResolve(new SetMoveIntentTransform(player, CardinalDirection.Right));

        // crate has moved, player has not
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(1, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(crate).Position, new GridPosition(2, 0));

        ApplyAndResolve(new SetMoveIntentTransform(player, CardinalDirection.Right));

        // player has moved into the space that crate has vacated
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(2, 0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(crate).Position, new GridPosition(2, 0));
    }

    public void PlayerPushesTwoAdjacentCrates()
    {
        
    }

    public void PlayerPushesGlass()
    {
    }

    public void PlayerPushesMultipleGlass()
    {
    }

    public void PlayerIsBlockedByWater()
    {
    }

    public void PushCrateIntoWaterAndWalkOnIt()
    {
    }

    public void MultipleObjectsMoveToSameSpotOnSameFrame()
    {
    }

    private void PrintStatus()
    {
    }
}