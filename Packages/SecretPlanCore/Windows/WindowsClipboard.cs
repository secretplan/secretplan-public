using System.Runtime.InteropServices;

namespace SecretPlanCore.Windows;

public static class WindowsClipboard
{
    private const uint CF_DIB = 8;
    private const uint GMEM_MOVEABLE = 0x0002;

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool EmptyClipboard();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool CloseClipboard();

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr bytes);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GlobalLock(IntPtr hMem);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GlobalUnlock(IntPtr hMem);
    
    
    public static void SetBgraImage(byte[] bgraPixels, int width, int height)
    {
        var dib = BuildDib(bgraPixels, width, height);

        var hGlobal = GlobalAlloc(GMEM_MOVEABLE, (UIntPtr)dib.Length);
        if (hGlobal == IntPtr.Zero)
        {
            throw new Exception("GlobalAlloc failed.");
        }

        var ptr = GlobalLock(hGlobal);
        if (ptr == IntPtr.Zero)
        {
            throw new Exception("GlobalLock failed.");
        }

        Marshal.Copy(dib, 0, ptr, dib.Length);
        GlobalUnlock(hGlobal);

        if (!OpenClipboard(IntPtr.Zero))
        {
            throw new Exception("OpenClipboard failed.");
        }

        try
        {
            if (!EmptyClipboard())
            {
                throw new Exception("EmptyClipboard failed.");
            }

            if (SetClipboardData(CF_DIB, hGlobal) == IntPtr.Zero)
            {
                throw new Exception("SetClipboardData failed.");
            }
        }
        finally
        {
            CloseClipboard();
        }
    }

    private static byte[] BuildDib(byte[] bgra, int width, int height)
    {
        const int BITMAPINFOHEADER_SIZE = 40;

        var headerSize = BITMAPINFOHEADER_SIZE;
        var pixelDataSize = bgra.Length;
        var dibSize = headerSize + pixelDataSize;

        var dib = new byte[dibSize];

        // BITMAPINFOHEADER
        void WriteInt32(int offset, int value)
        {
            Array.Copy(BitConverter.GetBytes(value), 0, dib, offset, 4);
        }

        WriteInt32(0, 40); // biSize
        WriteInt32(4, width); // biWidth
        WriteInt32(8, height); // biHeight (positive = bottom-up)
        WriteInt32(12, 1 | (32 << 16)); // biPlanes=1, biBitCount=32
        WriteInt32(16, 0); // BI_RGB (no compression)
        WriteInt32(20, pixelDataSize);
        WriteInt32(24, 0); // biXPelsPerMeter
        WriteInt32(28, 0); // biYPelsPerMeter
        WriteInt32(32, 0); // biClrUsed
        WriteInt32(36, 0); // biClrImportant

        // Pixel data (BGRA)
        Buffer.BlockCopy(bgra, 0, dib, headerSize, bgra.Length);

        return dib;
    }
}