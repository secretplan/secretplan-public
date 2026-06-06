namespace SokoGame.World;

/// <summary>
///     Values matter here, 0 is the least dense where 2 is the most dense
/// </summary>
public enum Density
{
    FloatsInAir = 0,
    FloatsInLiquid = 1,
    SinksInLiquid = 2
}