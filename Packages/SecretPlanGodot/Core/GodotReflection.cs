using Godot;

namespace SecretPlanGodot.Core;

// this could be extended into a doc-gen / code-gen platform.
public static class GodotReflection
{
    // todo: make classes for GodotMethod, GodotMethodArgs, and all the other dictionary types godot gives us. that would make APIs like this way more ergonomic
    public static IEnumerable<string> GetMethodArgNames(GodotObject godotObject, string methodName)
    {
        var find = godotObject.GetMethodList().FirstOrDefault(a => a["name"].AsString() == methodName);
        if (find != null)
        {
            var items = find["args"].AsGodotArray().Select(a => a.AsGodotDictionary()["name"].AsString());
            return items;
        }

        return [];
    }
}