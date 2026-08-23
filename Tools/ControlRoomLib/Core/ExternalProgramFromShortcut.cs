namespace ControlRoomLib.Core;

public class ExternalProgramFromShortcut : ExternalProgram
{
    public ExternalProgramFromShortcut(string shortcut, string? workingDirectory, string? helpOnShortcutFail) : base(Platform.Shortcut(shortcut, helpOnShortcutFail),
        workingDirectory, shortcut)
    {
    }
}