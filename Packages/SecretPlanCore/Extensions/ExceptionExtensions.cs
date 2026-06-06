namespace SecretPlanCore.Extensions;

public static class ExceptionExtensions
{
    public static T? SearchInnerUntilFound<T>(this Exception self) where T : Exception
    {
        var inner = self;

        while (inner != null)
        {
            if (inner is T casted)
            {
                return casted;
            }

            inner = inner.InnerException;
        }

        return null;
    }
}