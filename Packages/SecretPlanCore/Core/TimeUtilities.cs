namespace SecretPlanCore.Core;

public static class TimeUtilities
{
    public static long TimeNowMilliseconds()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }
}