namespace ControlRoomLib.Programs;

public interface IFileExplorerProgram
{
    /// <summary>
    ///     Opens the resource (file or URL) at the path. If it's a file, it should open a file explorer, if it's a URL, it
    ///     should open a browser.
    /// </summary>
    Task Open(string path);
}