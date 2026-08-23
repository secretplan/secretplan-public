namespace SecretPlanCore.Configuration;

/// <summary>
///     If present, this class, struct or property will not generate a visualizer in the ConfigEditor
///     If on a field or property it will be COMPLETELY hidden, ConfigField.GetFieldsOfObject will not see it
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field)]
public class HideFromConfigEditorAttribute : Attribute;