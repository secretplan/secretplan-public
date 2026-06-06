using Newtonsoft.Json;

namespace SecretPlanCore.Core;

public class SecretPlanVersion
{
    [JsonProperty("semver")]
    public Version Version { get; set; } = new(0, 0, 0);
    
    /// <summary>
    /// The SHA from the source branch this build was cut from
    /// </summary>
    [JsonProperty("commit")]
    public string BuildSha { get; set; } = "0000000000000000000000000000000000000000";

    /// <summary>
    /// The branch this build came from (parent of the release branch)
    /// </summary>
    [JsonProperty("branch")]
    public string BuildBranch { get; set; } = "dev";

    [JsonIgnore]
    public string SemVerString => Version.ToString();

    public override string ToString()
    {
        return $"{Version} [{BuildBranch} - {ShortSha()}]";
    }

    public string ShortSha()
    {
        return BuildSha.Substring(0, Math.Min(7, BuildSha.Length));
    }

    public void BumpMajor()
    {
        Version = new Version(Version.Major + 1, 0, 0);
    }

    public void BumpMinor()
    {
        Version = new Version(Version.Major, Version.Minor + 1, 0);
    }

    public void BumpPatch()
    {
        Version = new Version(Version.Major, Version.Minor, Version.Build + 1);
    }

    public void StampGitInfo(string branch, string sha)
    {
        BuildBranch = branch;
        BuildSha = sha;
    }

    /// <summary>
    ///     Short representation of the version, this might get sent over the wire so please avoid changing this!
    /// </summary>
    public string NetworkString()
    {
        return $"{Version}:{ShortSha()}";
    }
}