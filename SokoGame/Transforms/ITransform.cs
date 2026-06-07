using ExTween;
using SokoGame.Animation;
using SokoGame.World;

namespace SokoGame.Transforms;

public interface ITransform
{
    /// <summary>
    ///     DOES NOT GUARANTEE CLONING!
    /// </summary>
    Frame ApplyTo(Frame frame);

    void BuildAnimation(MultiplexTween tween, EntityViewTable table);
}