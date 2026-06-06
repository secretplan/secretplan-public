using System.Diagnostics.Contracts;
using Godot;

namespace SecretPlanGodot.Core;

public static class AudioEffectExtensions
{
    /// <summary>
    /// WARNING: this is not a "pure" method, calling this will "eat" that chunk of the buffer. If multiple callers want the same buffer, cache the answer and use it in both places
    /// </summary>
    /// <param name="capture"></param>
    /// <returns></returns>
    public static float? GetAverageLinearVolume(this AudioEffectCapture? capture)
    {
        if (capture == null)
        {
            return null;
        }
        
        if (!capture.CanGetBuffer(1))
        {
            return null;
        }
        
        var size = Math.Min(1024, capture.GetFramesAvailable());
        
        var frames = capture.GetBuffer(size);

        if (frames.Length == 0)
        {
            return 0f;
        }

        var sum = new Vector2();
        foreach (var frame in frames)
        {
            // take absolute value of each sample
            sum += frame.Abs();
        }

        var average = sum / frames.Length;

        // whichever channel is louder is the "correct" one
        return Math.Max(average.X, average.Y);
    }
}