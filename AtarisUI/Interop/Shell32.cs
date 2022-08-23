using System;
using System.Runtime.InteropServices;

namespace AtarisUI.Interop
{
    class Shell32
    {
        [DllImport("shell32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int SHAppBarMessage(AppBarMessage dwMessage, ref APPBARDATA pData);
    }
}
