using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace SecretPlanCore.Core;

public static class ByteMarshaller
{
    /// <summary>
    ///     Convert a struct to bytes
    /// </summary>
    /// <typeparam name="T">An unmanaged struct that should have [StructLayout(LayoutKind.Sequential, Pack = 1)]</typeparam>
    /// <returns></returns>
    public static byte[] ToBytes<T>(T structure) where T : unmanaged
    {
        var span = MemoryMarshal.CreateReadOnlySpan(ref structure, 1);
        return MemoryMarshal.AsBytes(span).ToArray();
    }

    public static T FromBytes<T>(byte[] bytes) where T : unmanaged
    {
        ReadOnlySpan<byte> span = bytes;
        return MemoryMarshal.Read<T>(span);
    }

    public static bool HasDifference(byte[] newBytes, byte[] oldBytes)
    {
        return !newBytes.SequenceEqual(oldBytes);
    }

    public static void SetStringValue(ref FixedBytes256 buffer, string text)
    {
        Span<byte> span = buffer;
        span.Clear();

        Encoding.UTF8.GetBytes(text, span);
    }

    public static string GetStringValue(FixedBytes256 buffer)
    {
        ReadOnlySpan<byte> span = buffer;

        var len = span.IndexOf((byte)0);
        if (len < 0)
        {
            len = span.Length;
        }

        return Encoding.UTF8.GetString(span[..len]);
    }

    [InlineArray(256)]
    public struct FixedBytes256
    {
        private byte _element0;
        
        public static implicit operator string(FixedBytes256 bytes)
        {
            return GetStringValue(bytes);
        }
        
        public static implicit operator FixedBytes256(string str)
        {
            var buffer = new FixedBytes256();
            SetStringValue(ref buffer, str);
            return buffer;
        }
    }
}