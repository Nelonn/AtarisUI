using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace AtarisUI.Widgets
{
    /// <summary>
    /// Interaction logic for Bluetooth.xaml
    /// </summary>
    public partial class Bluetooth : UserControl, IWidget
    {
        public Bluetooth(dynamic settings)
        {
            InitializeComponent();

            this.Loaded += OnLoaded;

            //enumerateSnapshot();
        }

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in content.Children)
            {
                if (element is Border border)
                {
                    border.MouseEnter += delegate
                    {
                        border.Background = new SolidColorBrush(Color.FromArgb(30, 255, 255, 255));
                    };
                    border.MouseLeave += delegate
                    {
                        border.Background = null;
                    };
                }
            }
        }

        /*async void enumerateSnapshot()
        {
            //System.Windows.Forms.Screen;

            //DeviceInformationCollection collection = await DeviceInformation.FindAllAsync();

            //MessageBox.Show(collection.Count() + "");
        }*/

        public void Button_Settings(object sender, MouseButtonEventArgs e)
        {
            Process.Start("ms-settings:bluetooth");
        }

        public new void MouseLeftButtonDown()
        {

        }

        public new void MouseLeftButtonUp()
        {
            //Process.Start("ms-settings:bluetooth");
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
