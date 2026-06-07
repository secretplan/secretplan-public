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

    protected TransformGroupAnimated ResolveCurrentFrame()
    {
        var resolveTransform = CurrentFrame.GetResolveTransform();
        CurrentFrame = CurrentFrame.CloneWithTransform(resolveTransform);
        return resolveTransform;
    }

    protected TransformGroupAnimated ApplyAndResolve(ITransform transform)
    {
        CurrentFrame.Log($"TEST: Running {nameof(ApplyAndResolve)}");
        ApplyTransform(transform);
        return ResolveCurrentFrame();
    }

    public void EnableLogging(string prefix)
    {
        GlobalDebug.LoggingPrefix = prefix;
        GlobalDebug.IsLoggingEnabled = true;
    }
}