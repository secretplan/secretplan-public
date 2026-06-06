using SokoGame.World;

namespace SokoGame.Transforms;

public readonly record struct DoNothingTransform : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        return frame;
    }

    public override string ToString()
    {
        return "DO_NOTHING";
    }
}