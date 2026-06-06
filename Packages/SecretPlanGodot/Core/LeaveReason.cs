namespace SecretPlanGodot.Core;

public enum LeaveReason
{
    UserRequest,
    KickedByHost,
    LostConnectionToHost,
    ConnectionFailed,
    CouldNotBuildENetClient,
    LobbyCreateFailed,
    VersionMismatch,
    SteamLobbyJoinFailed,
    CouldNotBuildSteamClient,
    CouldNotGetLobbyData
}