using SecretPlanCore.Core;

namespace FriendShapedDistributable;

public static class Global
{
    public static NoiseBasedRng DirtyRandom = new((int)TimeUtilities.TimeNowMilliseconds());
    public static NoiseBasedRng CleanRandom = new((int)TimeUtilities.TimeNowMilliseconds());
    
}