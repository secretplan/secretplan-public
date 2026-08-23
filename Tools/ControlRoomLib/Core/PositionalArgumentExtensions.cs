using SecretPlanCore.ArgumentParsing;

namespace ControlRoomLib.Core;

public static class PositionalArgumentExtensions
{
    public static BuildTarget ParseAsBuildTarget(this PositionalArgument argument)
    {
        var allOptions = BuildTarget.AllAvailable().ToList();
        var possibleStrings = allOptions.Select(a => a.ShortHandName);
        var foundShorthandName = argument.ParseAsSpecificString(possibleStrings.ToArray());

        foreach (var option in allOptions)
        {
            // Should be OK to do direct == here since we should have gotten back an exact match
            if (option.ShortHandName == foundShorthandName)
            {
                return option;
            }
        }

        throw new Exception(
            $"{nameof(ParseAsBuildTarget)} parsed successfully but couldn't find a matching option, this shouldn't be possible.");
    }
}