using SokoCore;
using SokoGame;
using SokoGame.Transforms;
using SokoGame.World;

namespace SokoConsole2;

public class BasicTests : BaseTests
{
    public void PlayerMovesInEmptyVoid()
    {
        var player = StartingFrame.AddEntity(new Entity { Position = GridPosition.Zero });

        ApplyAndResolve(new SetMoveIntentTransform(player, CardinalDirection.Right));

        SimpleAssert.ShouldBe(StartingFrame.GetEntity(player).Position, GridPosition.Zero);
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(1, 0));
    }

    public void PlayerHitsWall()
    {
        var player = StartingFrame.AddEntity(new Entity { Position = GridPosition.Zero });
        var wall = StartingFrame.AddEntity(new Entity { Position = new GridPosition(2, 0) });
        
        ApplyAndResolve(new SetMoveIntentTransform(player, CardinalDirection.Right));
        ApplyAndResolve(new SetMoveIntentTransform(player, CardinalDirection.Right));
        
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(player).Position, new GridPosition(1, 0));
    }

    public void PlayerPushesCrate()
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