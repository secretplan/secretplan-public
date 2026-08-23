namespace SecretPlanGodot.Core;

public interface IStateProvider<out TState>
{
    public TState State { get; }
}