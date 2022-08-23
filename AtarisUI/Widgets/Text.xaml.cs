using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AtarisUI.Widgets
{
    /// <summary>
    /// Interaction logic for Text.xaml
    /// </summary>
    public partial class Text : UserControl, IWidget
    {
        private string color = "#ffffff";
        private string text = "";
        private int fontSize = 14;

        public Text(dynamic settings)
        {
            InitializeComponent();

            if (settings["color"] != null) this.color = (string)settings["color"];
            if (settings["text"] != null) this.text = (string)settings["text"];
            if (settings["font_size"] != null) this.fontSize = (int)settings["font_size"];

            textBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            textBlock.Text = text;
            textBlock.FontSize = fontSize;
        }

        public new void MouseLeftButtonDown()
        {

        }

        public new void MouseLeftButtonUp()
        {

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
