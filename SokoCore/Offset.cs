namespace SokoCore;

public readonly record struct Offset(int X, int Y)
{
    public static Offset Zero { get; set; }

    public static Offset operator +(Offset left, Offset right)
    {
        return new Offset(left.X + right.X, left.Y + right.Y);
    }

    public static Offset operator -(Offset left, Offset right)
    {
        return new Offset(left.X - right.X, left.Y - right.Y);
    }

    public static Offset operator *(Offset offset, int scaler)
    {
        return new Offset(offset.X * scaler, offset.Y + scaler);
    }

    public static Offset operator -(Offset offset)
    {
        return new Offset(-offset.X, -offset.Y);
    }
    
    public static implicit operator Offset(CardinalDirection direction)
    {
        return FromDirection(direction);
    }

    public static Offset FromDirection(CardinalDirection direction)
    {
        switch (direction)
        {
            case CardinalDirection.Right:
                return new Offset(1, 0);
            case CardinalDirection.Up:
                return new Offset(0, -1);
            case CardinalDirection.Left:
                return new Offset(-1, 0);
            case CardinalDirection.Down:
                return new Offset(0, 1);
        }

        return Zero;
    }

    public override string ToString()
    {
        return $"Offset<{X},{Y}>";
    }
}