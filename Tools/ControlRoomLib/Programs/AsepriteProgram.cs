using ControlRoomLib.Core;

namespace ControlRoomLib.Programs;

public class AsepriteProgram : ExternalProgramFromShortcut
{
    public AsepriteProgram(string? workingDirectory = null) : base("aseprite", workingDirectory, null)
    {
    }

    public async Task BuildAtlas(IList<string> asepriteFileNames, string outputFileNameNoExtension)
    {
        var args = new List<string>
        {
            "-b",
            "--sheet", $"{outputFileNameNoExtension}.png",
            "--data", $"{outputFileNameNoExtension}.json",
            "--sheet-pack",
            "--shape-padding", "2"
        };

        args.AddRange(asepriteFileNames);

        await Run(args.ToArray());
    }
}