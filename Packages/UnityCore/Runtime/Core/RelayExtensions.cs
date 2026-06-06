namespace SecretPlan.Core
{
    public static class RelayExtensions
    {
        public static TState StateSafe<TState>(this ScriptableRelay<TState>? nullableRelay) where TState : new()
        {
            if (nullableRelay == null)
            {
                return new TState();
            }

            return nullableRelay.State();
        }
    }
}