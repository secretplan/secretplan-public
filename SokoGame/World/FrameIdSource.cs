namespace SokoGame.World;

public class FrameIdSource
{
    private uint _currentFrameId;

    public uint NextFrameId()
    {
        return _currentFrameId++;
    }
}