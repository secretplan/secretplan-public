using SokoCore;
using SokoGame.Transforms;
using SokoGame.World;

namespace SokoConsole2;

public class AnimationTests : BaseTests
{
    public void AnimatePushingCrateIntoWater()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        var crate = StartingFrame.AddEntity(EntityTemplate.Crate(new GridPosition(1, 0)));
        var water = StartingFrame.AddEntity(EntityTemplate.Water(new GridPosition(2, 0)));

        // Push the crate into the water
        var resolveTransform = ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));

        foreach (var transform in resolveTransform.All())
        {
            Console.WriteLine(transform);
        }
    }

    public void AnimatePushingMultipleGlass()
    {
        var player = StartingFrame.AddEntity(EntityTemplate.Player(GridPosition.Zero));
        StartingFrame.AddEntity(EntityTemplate.GlassLightCrate(new GridPosition(1, 0)));
        StartingFrame.AddEntity(EntityTemplate.GlassLightCrate(new GridPosition(2, 0)));

        // Push the crate into the water
        var resolveTransform = ApplyAndResolve(new TransformSetMoveIntent(player, CardinalDirection.Right));
        
        foreach (var transform in resolveTransform.All())
        {
            Console.WriteLine(transform);
        }
    }
}