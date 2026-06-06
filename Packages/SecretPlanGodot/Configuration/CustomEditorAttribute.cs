namespace SecretPlanGodot.Configuration;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class CustomEditorAttribute : Attribute
{
    public CustomEditorAttribute(string scenePath)
    {
        Scene = new PackedSceneReference(scenePath);
    }

    public PackedSceneReference Scene { get; }
}