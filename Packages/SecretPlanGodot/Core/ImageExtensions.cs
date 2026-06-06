using Godot;

namespace SecretPlanGodot.Core;

public static class ImageExtensions
{
    /// <summary>
    /// Oddly specific function that gets the image in the exact format the Windows wants to put it on our clipboard
    /// </summary>
    /// <returns></returns>
    public static byte[] GetBgraBytesOfFlippedImage(this Image image)
    {
        image.FlipY();
        image.Convert(Image.Format.Rgba8);

        var rgba = image.GetData();

        var bgra = new byte[rgba.Length];
        for (var i = 0; i < rgba.Length; i += 4)
        {
            // RGBA -> BGRA
            bgra[i + 0] = rgba[i + 2];
            bgra[i + 1] = rgba[i + 1];
            bgra[i + 2] = rgba[i + 0];
            bgra[i + 3] = rgba[i + 3];
        }

        return bgra;
    }
}