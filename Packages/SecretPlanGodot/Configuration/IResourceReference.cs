namespace SecretPlanGodot.Configuration;

public interface IResourceReference
{
    public string Path { get; }
    
    /// <summary>
    ///     Underlying type of the Resource this wraps
    /// </summary>
    public Type ResourceType { get; }

    object? GetOrLoadOrNullTypeless();
}