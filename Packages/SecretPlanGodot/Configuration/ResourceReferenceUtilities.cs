using Godot;

namespace SecretPlanGodot.Configuration;

public static class ResourceReferenceUtilities
{
    public static IResourceReference CreateResourceReference(Type resourceReferenceType, string path)
    {
        if (Activator.CreateInstance(resourceReferenceType, path) is not IResourceReference instance)
        {
            throw new Exception($"Could not build {nameof(IResourceReference)} from {resourceReferenceType.FullName}");
        }

        return instance;
    }

    public static IEnumerable<string> GetFileExtensions(Type resourceReferenceType)
    {
        var resourceType = CreateResourceReference(resourceReferenceType, "").ResourceType;
        
        if (resourceType == typeof(Texture2D))
        {
            yield return ".png";
            yield return ".bmp";
            yield return ".tga";
        }

        if (resourceType == typeof(AudioStream))
        {
            yield return ".ogg";
            yield return ".wav";
        }

        if (resourceType == typeof(PackedScene))
        {
            yield return ".tscn";
        }
    }
}