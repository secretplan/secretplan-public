using ControlRoom.Core;
using ControlRoom.Programs;
using SecretPlanCore.Core;

namespace ControlRoom.Missions;

public class AsepriteAtlas : Mission
{
    public AsepriteAtlas(List<string> rawArgs) : base(rawArgs)
    {
    }

    public override async Task Run()
    {
        var fileNames = MissionVariables.GameDirectoryFiles().GetFilesAt("Art/Aseprite");

        await OutPipe.AgentLogMessage($"Atlasing {fileNames.Count} files");

        var aseprite = new AsepriteProgram(MissionVariables.GameDirectory);
        await aseprite.BuildAtlas(fileNames, "Art/FullAtlas");
    }
}