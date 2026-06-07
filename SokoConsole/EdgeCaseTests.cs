using SokoCore;
using SokoGame.Transforms;
using SokoGame.World;

namespace SokoConsole2;

public class EdgeCaseTests : BaseTests
{
    public void PushTwoStackedCratesAtSamePosition()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        var crate1 = StartingFrame.AddEntity(EntityTemplate.Crate(new GridPosition(1, 0)));
        var crate2 = StartingFrame.AddEntity(EntityTemplate.Crate(new GridPosition(1, 0)));
        
        ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));
        
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(crate1).Position, new GridPosition(2,0));
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(crate2).Position, new GridPosition(2,0));
    }

    public void MultipleObjectsMoveToSameSpotOnSameFrame()
    {
        var crate1 = StartingFrame.AddEntity(EntityTemplate.Crate(new GridPosition(-1, 0)));
        var crate2 = StartingFrame.AddEntity(EntityTemplate.Crate(new GridPosition(1, 0)));

        StartingFrame.SetEntity(crate1, StartingFrame.GetEntity(crate1) with { MoveIntent = CardinalDirection.Right });
        StartingFrame.SetEntity(crate2, StartingFrame.GetEntity(crate2) with { MoveIntent = CardinalDirection.Left });
        
        ResolveCurrentFrame();
        
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(crate1).Position, GridPosition.Zero);
        SimpleAssert.ShouldBe(CurrentFrame.GetEntity(crate2).Position, GridPosition.Zero);
    }
}