using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AtarisUI.Widgets
{
    /// <summary>
    /// Interaction logic for Shortcut.xaml
    /// </summary>
    public partial class Shortcut : UserControl, IWidget
    {
        private ProcessStartInfo startInfo = new ProcessStartInfo("explorer.exe");
        private string Icon = "";

        public Shortcut(dynamic settings)
        {
            InitializeComponent();

            if (settings["file"] != null) startInfo.FileName = (string)settings["file"];
            if (settings["arguments"] != null) startInfo.Arguments = (string)settings["arguments"];
            if (settings["working_directory"] != null) startInfo.WorkingDirectory = (string)settings["working_directory"];
            if (settings["title"] != null) this.ToolTip = (string)settings["title"];
            if (settings["icon"] != null) this.Icon = (string)settings["icon"];

            if (Icon.Length > 0)
                this.image.Source = new BitmapImage(new Uri(Icon));
            else
            {
                System.Drawing.Icon icon = this.IconFromFilePath(startInfo.FileName);
                if (icon != null)
                    image.Source = this.ToImageSource(icon);
            }
        }

        private System.Drawing.Icon IconFromFilePath(string path)
        {
            System.Drawing.Icon result = null;
            try
            {
                result = System.Drawing.Icon.ExtractAssociatedIcon(path);
            }
            catch { }
            return result;
        }

        public new void MouseLeftButtonDown()
        {

        }

        public new void MouseLeftButtonUp()
        {
            Process.Start(startInfo);
        }

        public new void MouseRightButtonDown()
        {

        }

        public new void MouseRightButtonUp()
        {

        }

        public void Dispose()
        {

        }

        public FrameworkElement GetComponent()
        {
            return this;
        }

        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        private ImageSource ToImageSource(System.Drawing.Icon icon)
        {
            System.Drawing.Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new Win32Exception();
            }

            return wpfBitmap;
        }
    }
}
