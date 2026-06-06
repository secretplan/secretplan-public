using Newtonsoft.Json;
using SecretPlanCore.ArgumentParsing;
using SecretPlanCore.Core;

namespace ControlRoom.Core;

public record MissionVariables : IArgumentBundle
{
    [JsonProperty("gameDirectory")]
    [Argument("gameDirectory")]
    public string? GameDirectory { get; set; }

    public IFileSystem GameDirectoryFiles()
    {
        return new RealFileSystem(GameDirectory ?? ".");
    }
}