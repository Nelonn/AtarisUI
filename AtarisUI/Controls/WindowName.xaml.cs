using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AtarisUI.Controls
{
    /// <summary>
    /// Interaction logic for WindowName.xaml
    /// </summary>
    public partial class WindowName : UserControl, IWidget
    {
        public WindowName()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;

            this.menu.Child.PreviewMouseUp += delegate
            {
                menu.IsOpen = false;
            };
        }

        public void SetText(string NewText)
        {
            text.Text = NewText;
        }

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.Width = double.NaN;
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

        private void Button_Preferences(object sender, MouseButtonEventArgs e)
        {
            WindowManager.CreateOrFocusWindow<SettingsWindow>();
        }

        private void Button_HideTaskbar(object sender, MouseButtonEventArgs e)
        {
            Process.Start("ms-settings:about");
        }

        private void Button_ShowTaskbar(object sender, MouseButtonEventArgs e)
        {
            Process.Start("ms-settings:about");
        }

        private void Button_ShowConfig(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", "/select, \"" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AtarisUI", "config.json") + "\"");
        }

        private void Button_Reload(object sender, MouseButtonEventArgs e)
        {
            TopBarWindow.Instance.Reload();
        }

        private void Button_Exit(object sender, MouseButtonEventArgs e)
        {
            TopBarWindow.Instance.Close();
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
