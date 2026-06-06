using System.Security.Cryptography;
using System.Text;

namespace SecretPlanCore.Core;

public static class Hashing
{
    /// <summary>
    ///     Adapted from https://gist.github.com/badboy/6267743
    /// </summary>
    public static ulong Hash64(ulong key)
    {
        key = ~key + (key << 21); // key = (key << 21) - key - 1;
        key = key ^ (key >>> 24);
        key = key + (key << 3) + (key << 8); // key * 265
        key = key ^ (key >>> 14);
        key = key + (key << 2) + (key << 4); // key * 21
        key = key ^ (key >>> 28);
        key = key + (key << 31);
        return key;
    }

    public static ulong InverseHash64(ulong key)
    {
        ulong tmp;

        // Invert key = key + (key << 31)
        tmp = key - (key << 31);
        key = key - (tmp << 31);

        // Invert key = key ^ (key >> 28)
        tmp = key ^ (key >> 28);
        key = key ^ (tmp >> 28);

        // Invert key *= 21
        key *= 14933078535860113213u;

        // Invert key = key ^ (key >> 14)
        tmp = key ^ (key >> 14);
        tmp = key ^ (tmp >> 14);
        tmp = key ^ (tmp >> 14);
        key = key ^ (tmp >> 14);

        // Invert key *= 265
        key *= 15244667743933553977u;

        // Invert key = key ^ (key >> 24)
        tmp = key ^ (key >> 24);
        key = key ^ (tmp >> 24);

        // Invert key = (~key) + (key << 21)
        tmp = ~key;
        tmp = ~(key - (tmp << 21));
        tmp = ~(key - (tmp << 21));
        key = ~(key - (tmp << 21));

        return key;
    }

    /// <summary>
    ///     Implementation of MurmurHash3, not meant to be a secure hash, just a way to quickly boil a string into a number
    /// </summary>
    public static uint HashString(string input, uint seed = 0)
    {
        var data = Encoding.UTF8.GetBytes(input);
        const uint c1 = 0xcc9e2d51;
        const uint c2 = 0x1b873593;

        var hash = seed;
        var length = data.Length;
        var roundedEnd = length & ~0x3;

        for (var i = 0; i < roundedEnd; i += 4)
        {
            var k = BitConverter.ToUInt32(data, i);
            k *= c1;
            k = (k << 15) | (k >> 17);
            k *= c2;

            hash ^= k;
            hash = (hash << 13) | (hash >> 19);
            hash = hash * 5 + 0xe6546b64;
        }

        uint tail = 0;
        switch (length & 3)
        {
            case 3:
                tail ^= (uint)data[roundedEnd + 2] << 16;
                goto case 2;
            case 2:
                tail ^= (uint)data[roundedEnd + 1] << 8;
                goto case 1;
            case 1:
                tail ^= data[roundedEnd];
                tail *= c1;
                tail = (tail << 15) | (tail >> 17);
                tail *= c2;
                hash ^= tail;
                break;
        }

        hash ^= (uint)length;
        hash ^= hash >> 16;
        hash *= 0x85ebca6b;
        hash ^= hash >> 13;
        hash *= 0xc2b2ae35;
        hash ^= hash >> 16;

        return hash;
    }
        public static string HashStringToColor(string input)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
    
            var r = hash[0];
            var g = hash[1];
            var b = hash[2];
    
            const double minBrightness = 0.5;
            var brightness = (0.299 * r + 0.587 * g + 0.114 * b) / 255.0;
            if (brightness < minBrightness)
            {
                var factor = minBrightness / brightness;
                r = (byte)Math.Min(255, r * factor);
                g = (byte)Math.Min(255, g * factor);
                b = (byte)Math.Min(255, b * factor);
            }
    
            return $"#{r:X2}{g:X2}{b:X2}";
        }
}