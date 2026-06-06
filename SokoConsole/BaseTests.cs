using SokoGame;
using SokoGame.Transforms;
using SokoGame.World;

namespace SokoConsole2;

public class BaseTests
{
    public BaseTests()
    {
        StartingFrame = new Frame(new FrameIdSource());
        CurrentFrame = StartingFrame;
    }

    protected Frame CurrentFrame { get; private set; }
    protected Frame StartingFrame { get; }
    
    protected void ApplyTransform(ITransform transform)
    {
        CurrentFrame = CurrentFrame.CloneWithTransform(transform);
    }

    protected void ResolveCurrentFrame()
    {
        CurrentFrame = CurrentFrame.CloneWithTransform(CurrentFrame.GetResolveTransform());
    }

    protected void ApplyAndResolve(ITransform transform)
    {
        ApplyTransform(transform);
        ResolveCurrentFrame();
    }

    public void EnableLogging(string prefix)
    {
        Global.LoggingPrefix = prefix;
        Global.IsLoggingEnabled = true;
    }
}