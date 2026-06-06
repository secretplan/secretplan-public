using System.Text;
using SokoGame.World;

namespace SokoGame.Transforms;

public class TransformGroup : ITransform
{
    private readonly List<ITransform> _transforms = new();

    public void Add(ITransform transform)
    {
        _transforms.Add(transform);
    }

    public IEnumerable<ITransform> All()
    {
        return _transforms;
    }

    public Frame ApplyTo(Frame frame)
    {
        var result = frame;
        foreach (var transform in _transforms)
        {
            result = transform.ApplyTo(result);
        }

        return result;
    }

    public bool IsEmpty()
    {
        return _transforms.Count == 0 || _transforms.All(transform => transform.IsNoOp());
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(GetType().Name);
        stringBuilder.Append("[");
        stringBuilder.Append(string.Join(", ", _transforms.Select(a => a.ToString())));
        stringBuilder.Append("]");
        return stringBuilder.ToString();
    }
}