using SecretPlanCore.Core;

namespace SokoGame.Animation;

public static class Global
{
    public static readonly NoiseBasedRng Random = new((int)TimeUtilities.TimeNowMilliseconds());
}