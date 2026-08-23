using System.Text;

namespace SecretPlanGodot.Core;

public class LoadingStatus
{
    private readonly HashSet<string> _namedFlags = new();

    /// <summary>
    ///     Greater than 0 means "Show loading scrim"
    /// </summary>
    public int NumberedFlagValue { get; private set; }

    /// <summary>
    ///     Proxy for external consumers as "is showing loading scrim"
    /// </summary>
    public bool ShouldShowLoadingScrim => NumberedFlagValue > 0 || _namedFlags.Count > 0;

    public IEnumerable<string> AllNamedFlags()
    {
        return _namedFlags;
    }
    
    public event Action? Showed;
    public event Action? Hid;
    public event Action? Changed;

    public void IncrementLoadingScrim()
    {
        var old = ShouldShowLoadingScrim;
        NumberedFlagValue++;
        NotifyUpdate(old);

        LocalClient.Print($"LOAD: Loading Scrim numbered flags incremented; now set to: {NumberedFlagValue}");
    }

    public void DecrementLoadingScrim()
    {
        var old = ShouldShowLoadingScrim;
        NumberedFlagValue--;
        if (NumberedFlagValue < 0)
        {
            NumberedFlagValue = 0;
        }

        NotifyUpdate(old);

        LocalClient.Print($"LOAD: Loading Scrim numbered flags decremented; now set to: {NumberedFlagValue}");
    }

    private void NotifyUpdate(bool previousState)
    {
        if (previousState != ShouldShowLoadingScrim)
        {
            if (ShouldShowLoadingScrim)
            {
                Showed?.Invoke();
            }
            else
            {
                Hid?.Invoke();
            }
        }

        Changed?.Invoke();
    }

    public void AddLoadingScrimFlag(string name)
    {
        var old = ShouldShowLoadingScrim;
        _namedFlags.Add(name);
        NotifyUpdate(old);
        LocalClient.Print($"LOAD: Loading Scrim flag {name} set");
    }

    public void RemoveLoadingScrimFlag(string name)
    {
        var old = ShouldShowLoadingScrim;
        _namedFlags.Remove(name);
        NotifyUpdate(old);
        LocalClient.Print($"LOAD: Loading Scrim flag {name} unset");
    }

    public string Status()
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("Numbered flag count: " + NumberedFlagValue);

        foreach (var flag in _namedFlags)
        {
            stringBuilder.AppendLine($"Named flag set: {flag}");
        }

        return stringBuilder.ToString();
    }

    public void FullClear()
    {
        var old = ShouldShowLoadingScrim;
        _namedFlags.Clear();
        NumberedFlagValue = 0;
        NotifyUpdate(old);
    }
}