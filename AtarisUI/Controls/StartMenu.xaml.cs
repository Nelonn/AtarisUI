using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AtarisUI
{
    /// <summary>
    /// Interaction logic for StartMenu.xaml
    /// </summary>
    public partial class StartMenu : UserControl, IWidget
    {
        public StartMenu()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;

            this.menu.Child.PreviewMouseUp += delegate
            {
                menu.IsOpen = false;
            };
        }

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in content.Children)
            {
                if (element is Border border)
                {
                    border.MouseEnter += delegate
                    {
                        border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1a62e6"));
                    };
                    border.MouseLeave += delegate
                    {
                        border.Background = null;
                    };
                }
            }
        }

        private void Button_About(object sender, MouseButtonEventArgs e)
        {
            Process.Start("ms-settings:about");
        }

        private void Button_Settings(object sender, MouseButtonEventArgs e)
        {
            Process.Start("ms-settings:system");
        }

        private void Button_TaskManager(object sender, MouseButtonEventArgs e)
        {
            Process.Start("Taskmgr.exe");
        }

        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        private void Button_Sleep(object sender, MouseButtonEventArgs e)
        {
            SetSuspendState(false, true, true);
        }

        private void Button_Reboot(object sender, MouseButtonEventArgs e)
        {
            Process.Start("shutdown", "/r /t 0");
        }

        private void Button_Shutdown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("shutdown", "/s /t 0");
        }

        [DllImport("user32")]
        public static extern void LockWorkStation();

        private void Button_LockScreen(object sender, MouseButtonEventArgs e)
        {
            LockWorkStation();
        }

        [DllImport("user32")]
        public static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        private void Button_LogOut(object sender, MouseButtonEventArgs e)
        {
            ExitWindowsEx(0, 0);
        }

        public new void MouseLeftButtonDown()
        {

        }

        public new void MouseLeftButtonUp()
        {
            menu.IsOpen = true;
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
    }
}
