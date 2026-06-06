using Godot;

namespace SecretPlanGodot.Core;

public readonly struct NavigationNeighbor(Control? control, NavigationNeighbor.NeighborType navigationType)
{
    public enum NeighborType
    {
        ExactControl = 0,
        Self = 1,
        LetGodotDecide = 2
    }

    public static implicit operator NavigationNeighbor(Control? givenControl)
    {
        return new NavigationNeighbor(givenControl, NeighborType.ExactControl);
    }

    public static readonly NavigationNeighbor Self = new(null, NeighborType.Self);
    public static readonly NavigationNeighbor LetGodotDecide = new(null, NeighborType.LetGodotDecide);

    public NodePath? GetPath(Control self)
    {
        return navigationType switch
        {
            NeighborType.ExactControl when control.IsValid() => control.GetPath(),
            NeighborType.Self => self.GetPath(),
            _ => null
        };
    }
}