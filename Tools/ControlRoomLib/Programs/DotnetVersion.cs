namespace ControlRoomLib.Programs;

public readonly record struct DotnetVersion(string FlagName)
{
    public static DotnetVersion Net8 => new DotnetVersion("net8.0");
}