using SokoGame.World;

namespace SokoGame.Transforms;

public interface ITransform
{
    /// <summary>
    /// DOES NOT GUARANTEE CLONING!
    /// </summary>
    Frame ApplyTo(Frame frame);
}