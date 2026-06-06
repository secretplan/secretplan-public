namespace SokoGame;

public static class Global
{
    public static bool IsLoggingEnabled;
    public static string LoggingPrefix = "DEBUG";

    public static void DebugLog(string message)
    {
        if (IsLoggingEnabled)
        {
            Console.WriteLine(LoggingPrefix + ": " + message);
        }
    }
}