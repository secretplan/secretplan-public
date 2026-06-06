namespace ControlRoom.Core;

public class ProgramFromShortcut : ExternalProgram
{
    public ProgramFromShortcut(string shortcut, string? workingDirectory, string? helpOnShortcutFail) : base(Platform.Shortcut(shortcut, helpOnShortcutFail),
        workingDirectory, shortcut)
    {
    }
}