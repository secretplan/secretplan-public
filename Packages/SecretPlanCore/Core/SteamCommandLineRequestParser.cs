namespace SecretPlanCore.Core;

public class SteamCommandLineRequestParser
{
    public static string? GetValueOfCommand(List<string> commandLineArgs, string commandName)
    {
        var indexOfCommand = commandLineArgs.IndexOf(commandName);

        if (indexOfCommand != -1)
        {
            if (commandLineArgs.IsValidIndex(indexOfCommand + 1))
            {
                return commandLineArgs[indexOfCommand + 1];
            }
        }

        return null;
    }
}