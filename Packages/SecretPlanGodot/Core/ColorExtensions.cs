using Godot;

namespace SecretPlanGodot.Core;

public static class ColorExtensions
{
    public static float GetRgbChannel(this Color color, RgbChannel channel)
    {
        switch (channel)
        {
            case RgbChannel.Red:
                return color.R;
            case RgbChannel.Green:
                return color.G;
            case RgbChannel.Blue:
                return color.B;
        }

        return 0;
    }

    public static void SetRgbChannelNormalized(this ref Color color, RgbChannel channel, float value)
    {
        switch (channel)
        {
            case RgbChannel.Red:
                color.R = value;
                break;
            case RgbChannel.Green:
                color.G = value;
                break;
            case RgbChannel.Blue:
                color.B = value;
                break;
        }
    }

    public static float GetHsvChannel(this Color color, HsvChannel channel)
    {
        color.ToHsv(out var hue, out var lightness, out var saturation);
        switch (channel)
        {
            case HsvChannel.Hue:
                return hue;
            case HsvChannel.Saturation:
                return saturation;
            case HsvChannel.Lightness:
                return lightness;
        }

        return 0;
    }

    public static void SetHsvChannelNormalized(this ref Color color, HsvChannel channel, float value)
    {
        color.ToHsv(out var hue, out var lightness, out var saturation);

        switch (channel)
        {
            case HsvChannel.Hue:
                hue = value;
                break;
            case HsvChannel.Saturation:
                saturation = value;
                break;
            case HsvChannel.Lightness:
                lightness = value;
                break;
        }

        var result = Color.FromHsv(hue, saturation, lightness, color.A);
        color.A = result.A;
        color.R = result.R;
        color.G = result.G;
        color.B = result.B;
    }

    public static HsvColor ToHsvColor(this Color color)
    {
        color.ToHsv(out var hue, out var lightness, out var saturation);
        return new HsvColor(hue, saturation, lightness, color.A);
    }
}