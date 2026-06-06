using SokoCore;
using SokoGame.World;

namespace SokoConsole2;

public class FrameTests : BaseTests
{
    public void WaterIsHole()
    {
        StartingFrame.AddEntity(EntityTemplate.Water(GridPosition.Zero));
        SimpleAssert.ShouldBe(StartingFrame.IsHoleAt(GridPosition.Zero), true);
        SimpleAssert.ShouldBe(StartingFrame.IsHoleAt(new GridPosition(1, 0)), false);
    }
    
    public void PitIsHole()
    {
        StartingFrame.AddEntity(EntityTemplate.Pit(GridPosition.Zero));
        SimpleAssert.ShouldBe(StartingFrame.IsHoleAt(GridPosition.Zero), true);
        SimpleAssert.ShouldBe(StartingFrame.IsHoleAt(new GridPosition(1, 0)), false);
    }
    
    public void FilledWaterIsNotHole()
    {
        StartingFrame.AddEntity(EntityTemplate.Water(GridPosition.Zero));
        StartingFrame.AddEntity(EntityTemplate.BrittleFloor(GridPosition.Zero));
        
        SimpleAssert.ShouldBe(StartingFrame.IsHoleAt(GridPosition.Zero), false);
    }
    
    public void FilledPitIsNotHole()
    {
        StartingFrame.AddEntity(EntityTemplate.Pit(GridPosition.Zero));
        StartingFrame.AddEntity(EntityTemplate.BrittleFloor(GridPosition.Zero));
        
        SimpleAssert.ShouldBe(StartingFrame.IsHoleAt(GridPosition.Zero), false);
    }
}