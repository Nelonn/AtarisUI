using AtarisUI.Interop;
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace AtarisUI
{
    /// <summary>
    /// Interaction logic for DockWindow.xaml
    /// </summary>
    public partial class DockWindow : Window
    {
        public dynamic Config;

        public DockWindow(dynamic config)
        {
            InitializeComponent();

            this.Config = config;

            this.WindowState = WindowState.Normal;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height - 24;
            this.Left = 0;
            this.Top = 24;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);

            int exStyle = (int)User32.GetWindowLong(wndHelper.Handle, (int)User32.GetWindowLongFields.GWL_EXSTYLE);

            //exStyle = exStyle | User32.WS_EX_TOOLWINDOW | User32.WS_EX_NOACTIVATE;
            exStyle = exStyle | User32.WS_EX_NOACTIVATE;
            User32.SetWindowLong(wndHelper.Handle, (int)User32.GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
        }
    }
}
