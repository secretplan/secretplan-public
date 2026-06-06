using System.Diagnostics;

namespace SecretPlanCore.MacOS;

public static class AppleScript
{
    public static string? RunEscaped(string appleScript)
    {
        var escapedScript = appleScript.Replace("\"", "\\\"");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "osascript",
                Arguments = $"-e \"{escapedScript}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        // Execute the process and capture the output
        process.Start();
        var result = process.StandardOutput.ReadToEnd().Trim();
        process.WaitForExit();

        if (string.IsNullOrEmpty(result))
        {
            return null;
        }

        return result;
    }
    
    /// <summary>
    /// Runs `osascript [args]`
    /// </summary>
    /// <param name="rawArgs"></param>
    /// <returns></returns>
    public static string? RunRaw(string rawArgs)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "osascript",
                Arguments = rawArgs,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        // Execute the process and capture the output
        process.Start();
        var result = process.StandardOutput.ReadToEnd().Trim();
        process.WaitForExit();

        if (string.IsNullOrEmpty(result))
        {
            return null;
        }

        return result;
    }
}