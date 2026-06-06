namespace SokoGame.Transforms;

public readonly record struct DoNothingTransform : ITransform
{
    public Frame ApplyTo(Frame frame)
    {
        return frame;
    }
}