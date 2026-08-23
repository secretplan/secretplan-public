using System.Diagnostics.Contracts;
using Godot;

namespace SecretPlanGodot.Core;

public class LoadingHandle
{
    private readonly Func<(ResourceLoader.ThreadLoadStatus, float)> _getStatusFunc;
    private readonly string _label;
    private readonly Action _onFinished;
    private readonly Func<Error> _start;
    private int _finishDelayFrames = 5;

    private int _startDelayFrames = 2;

    public LoadingHandle(string label, Func<Error> start, Func<(ResourceLoader.ThreadLoadStatus, float)> getStatusFunc,
        Action onFinished)
    {
        _label = label;
        _start = start;
        _getStatusFunc = getStatusFunc;
        _onFinished = onFinished;
    }

    public override string ToString()
    {
        return _label;
    }

    public ResourceLoader.ThreadLoadStatus Poll()
    {
        if (_startDelayFrames > 0)
        {
            _startDelayFrames--;

            if (_startDelayFrames == 0)
            {
                LocalClient.Print($"Starting load for: {_label}");
                var startResult = _start();

                if (startResult != Error.Ok)
                {
                    throw new Exception($"Async action failed to start {startResult}");
                }

                LocalClient.Print("Load started, will come back when it's finished");
            }

            return ResourceLoader.ThreadLoadStatus.InProgress;
        }

        var (status, _) = _getStatusFunc();
        if (status == ResourceLoader.ThreadLoadStatus.Loaded && _finishDelayFrames > 0)
        {
            _finishDelayFrames--;
            if (_finishDelayFrames == 0)
            {
                LocalClient.Print($"Finished load for: {_label}");
                _onFinished();
            }

            return ResourceLoader.ThreadLoadStatus.InProgress;
        }

        return status;
    }

    [Pure]
    public float GetPercent()
    {
        var (status, percent) = _getStatusFunc();
        return percent;
    }

    public static LoadingHandle Instant(string label)
    {
        return new LoadingHandle(label,
            () => Error.Ok,
            () => (ResourceLoader.ThreadLoadStatus.Loaded, 1f),
            () => { }
        );
    }
}
