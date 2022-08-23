using System;
using System.Runtime.InteropServices;

namespace AtarisUI.Interop
{
    class Gdi32
    {
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int w, int h);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);

        [DllImport("gdi32.dll", PreserveSig = true)]
        public static extern bool DeleteObject(IntPtr objectHandle);
    }
}
