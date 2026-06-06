namespace SecretPlan.Core
{
    public interface IValueProvider<out TData>
    {
        public TData Get();
    }
}