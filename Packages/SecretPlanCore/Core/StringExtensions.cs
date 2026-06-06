using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace SecretPlanCore.Core;

public static class StringExtensions
{
    /// <summary>
    ///     Splits a string, but treats parts of the string wrapped in quotes as its own token. eg: `1 2 "three four"   5`
    ///     becomes ["1", "2", "three four", "5"]
    /// </summary>
    /// <returns></returns>
    [Pure]
    public static string[] SplitTokens(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return [];
        }

        var tokens = new List<string>();

        var regex = new Regex("\"([^\"]*)\"|(\\S+)");
        foreach (Match match in regex.Matches(input))
        {
            if (match.Groups[1].Success)
            {
                tokens.Add(match.Groups[1].Value);
            }
            else
            {
                tokens.Add(match.Groups[2].Value);
            }
        }

        return tokens.ToArray();
    }


    [Pure]
    public static string[] SplitLines(this string str)
    {
        return str.Split(["\r\n", "\r", "\n"], StringSplitOptions.None);
    }

    [Pure]
    public static string[] SplitDirectorySeparators(this string str)
    {
        return str.Split(["\\", "/"], StringSplitOptions.None);
    }

    [Pure]
    public static string RemoveFileExtension(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        var fileInfo = new FileInfo(str);
        return str.Substring(0, str.Length - fileInfo.Extension.Length);
    }
}