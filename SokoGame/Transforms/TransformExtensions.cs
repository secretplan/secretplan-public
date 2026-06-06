namespace SokoGame.Transforms;

public static class TransformExtensions
{
    public static bool IsNoOp(this ITransform transform)
    {
        if (transform is TransformDoNothing)
        {
            return true;
        }

        if (transform is TransformGroup group)
        {
            return group.IsEmptyOrNoOp();
        }

        return false;
    }
}