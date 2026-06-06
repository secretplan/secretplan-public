namespace SecretPlanCore.Configuration;

/// <summary>
///     If present, this class or struct will not generate a visualizer in the ConfigEditor
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class HideFromConfigEditorAttribute : Attribute;