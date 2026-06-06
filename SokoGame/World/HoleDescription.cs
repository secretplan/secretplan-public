using SokoCore;

namespace SokoGame.World;

public class HoleDescription
{
    private readonly Phase? _phase;

    public HoleDescription(GridPosition gridPosition, HashSet<EntityWithId> holes)
    {
        Position = gridPosition;
        _phase = null;
        foreach (var hole in holes)
        {
            if (_phase == null || (_phase == Phase.Solid && hole.Entity.Phase == Phase.Liquid) ||
                hole.Entity.Phase == Phase.Air)
            {
                // Set current phase to (in priority order) null -> Solid -> Liquid -> Air 
                _phase = hole.Entity.Phase;
            }
        }
    }

    public GridPosition Position { get; }

    public bool IsValid => _phase.HasValue;

    public HoleInteraction GetHoleInteraction(Entity aboveEntity)
    {
        if (aboveEntity.Depth != WorldDepth.Player)
        {
            // If the entity isn't at Player depth, there's nothing to do
            return HoleInteraction.DoNothing;
        }

        if (aboveEntity.Density == Density.FloatsInAir)
        {
            // If the entity floats in air, it will simply stay suspended above the hole
            return HoleInteraction.DoNothing;
        }

        if (_phase == Phase.Liquid && aboveEntity.Density == Density.FloatsInLiquid)
        {
            // Fill the hole
            return HoleInteraction.Fill;
        }

        if (_phase == Phase.Liquid && aboveEntity.Density >= Density.SinksInLiquid)
        {
            return HoleInteraction.Sink;
        }

        return HoleInteraction.Sink;
    }
}

public enum HoleInteraction
{
    DoNothing,
    Fill,
    Sink
}