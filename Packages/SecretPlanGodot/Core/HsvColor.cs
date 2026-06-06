using Godot;

namespace SecretPlanGodot.Core;

public record struct HsvColor(float Hue, float Saturation, float Lightness, float Alpha = 1f)
{
    public Color ToColor()
    {
        return Color.FromHsv(Hue, Saturation, Lightness, Alpha);
    }

    public float GetChannel(HsvChannel channel)
    {
        switch (channel)
        {
            case HsvChannel.Hue:
                return Hue;
            case HsvChannel.Saturation:
                return Saturation;
            case HsvChannel.Lightness:
                return Lightness;
        }

        return 0;
    }

    public void SetChannel(HsvChannel channel, float value)
    {
        switch (channel)
        {
            case HsvChannel.Hue:
                Hue = value;
                break;
            case HsvChannel.Saturation:
                Saturation = value;
                break;
            case HsvChannel.Lightness:
                Lightness = value;
                break;
        }
    }
}