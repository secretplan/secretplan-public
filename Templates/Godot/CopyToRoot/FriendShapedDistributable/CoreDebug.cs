using SecretPlanGodot.Core;

namespace FriendShapedDistributable;

public class CoreDebug : SecretPlanDebugger
{
    [DebugValue("loguinavigation")]
    public bool LogUiNavigation { get; set; }

    public bool IsConsoleOpen { get; set; }
}