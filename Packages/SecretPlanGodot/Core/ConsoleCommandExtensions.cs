using Godot;
using SecretPlanCore.ArgumentParsing;
using SecretPlanCore.Core;

namespace SecretPlanGodot.Core;

public static class ConsoleCommandExtensions
{
    public static Vector3 ParseAsVector3(this PositionalArgument self)
    {
        var floats = SplitFloats(self);
        if (floats.Length == 3)
        {
            return new Vector3(floats[0], floats[1], floats[2]);
        }

        throw new ArgParseFailedTypeException<Vector3>(self);
    }

    private static float[] SplitFloats(PositionalArgument self)
    {
        var floats = self.OriginalToken.Split(",").Select(a=>
        {
            float.TryParse(a, out var result);
            return result;
        }).ToArray();
        return floats;
    }

    public static Vector2 ParseAsVector2(this PositionalArgument self)
    {
        var floats = SplitFloats(self);
        if (floats.Length == 2)
        {
            return new Vector2(floats[0], floats[1]);
        }

        throw new ArgParseFailedTypeException<Vector2>(self);
    }
}