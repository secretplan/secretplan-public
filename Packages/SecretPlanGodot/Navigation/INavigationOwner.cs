using System.Diagnostics.CodeAnalysis;
using Godot;
using SecretPlanGodot.Core;

namespace BirdGame.UI;

public interface INavigationOwner
{
    public Control? GetDefaultFocusNode();
}

public interface INavigationOwnerExtended : INavigationOwner
{
    /// <summary>
    ///     Used in the IsValidAndVisible code path
    /// </summary>
    bool IsValidNavigationOwner();
}

public static class NavigationOwnerExtensions
{
    /// <summary>
    ///     Returns true if the navigation owner is not null and (if it's a Node under the hood) is Valid.
    /// </summary>
    public static bool IsValidAndVisible([NotNullWhen(true)] this INavigationOwner? navigationOwner)
    {
        if (navigationOwner == null)
        {
            return false;
        }

        if (navigationOwner is INavigationOwnerExtended extended)
        {
            return extended.IsValidNavigationOwner();
        }

        if (navigationOwner is Control control)
        {
            return control.IsValidAndNotQueuedForDeletion(); // && control.Visible; // sometimes it's valid to not be visible (eg: guidebook is not visible for the first few frames)
        }

        if (navigationOwner is Node node)
        {
            return node.IsValidAndNotQueuedForDeletion();
        }

        return true;
    }

    public static string DebugName(this INavigationOwner? owner)
    {
        var result = owner?.ToString() ?? "null";

        if (owner is Node node)
        {
            result = node.Name;
        }

        return result;
    }

    public static INavigationOwner? GetNavigationOwner(this Control? control)
    {
        if (!control.IsValidAndNotQueuedForDeletion())
        {
            return null;
        }

        return control.ClimbAncestorsUntilFindType<INavigationOwner>();
    }
}