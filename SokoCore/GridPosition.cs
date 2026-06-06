namespace SokoCore;

public readonly record struct GridPosition(int X, int Y)
{
    public static GridPosition Zero { get; } = new();

    public static GridPosition operator +(GridPosition gridPosition, CardinalDirection direction)
    {
        var offset = Offset.FromDirection(direction);
        return gridPosition + offset;
    }

    public static GridPosition operator +(GridPosition gridPosition, Offset offset)
    {
        return new GridPosition(gridPosition.X + offset.X, gridPosition.Y + offset.Y);
    }

    public override string ToString()
    {
        return $"Position<{X},{Y}>";
    }
}