using System;
using System.Runtime.InteropServices;

namespace AtarisUI.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    struct APPBARDATA
    {
        public uint cbSize;
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public AppBarEdge uEdge;
        public RECT rect;
        public int lParam;
    }
}
