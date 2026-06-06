using Godot;

namespace SecretPlanGodot.Core;

public static class ResourceExtensions
{
    /// <summary>
    /// Only reliably works in Editor if this is a Resource or Scene file.
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    public static long GetEditorUidLong(this Resource resource)
    {
        return ResourceLoader.GetResourceUid(resource.ResourcePath);
    }

    public static string GetEditorUidString(this Resource resource)
    {
        return ResourceUid.PathToUid(resource.ResourcePath);
    }
}