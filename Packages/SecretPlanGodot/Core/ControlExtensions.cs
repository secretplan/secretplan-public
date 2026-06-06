using Godot;

namespace SecretPlanGodot.Core;

public static class ControlExtensions
{
    public static void SetFocusNeighbors(this Control self, NavigationNeighbor top, NavigationNeighbor right,
        NavigationNeighbor bottom,
        NavigationNeighbor left)
    {
        self.SetFocusNeighbor(Side.Top, top.GetPath(self));
        self.SetFocusNeighbor(Side.Left, left.GetPath(self));
        self.SetFocusNeighbor(Side.Right, right.GetPath(self));
        self.SetFocusNeighbor(Side.Bottom, bottom.GetPath(self));
    }
}