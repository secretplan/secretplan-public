namespace SokoGame.Transforms;

public static class TransformExtensions
{
    public static bool IsNoOp(this ITransform transform)
    {
        if (transform is DoNothingTransform)
        {
            return true;
        }

        if (transform is TransformGroup group)
        {
            return group.IsEmpty();
        }

        return false;
    }
}