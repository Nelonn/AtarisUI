using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace AtarisUI.Widgets
{
    /// <summary>
    /// Interaction logic for Clock.xaml
    /// </summary>
    public partial class Clock : UserControl, IWidget
    {
        private string Format = "MMM d h:m tt";
        private string TextColor = "#FFFFFF";
        private Timer timer;

        public Clock(dynamic settings)
        {
            InitializeComponent();

            if (settings["format"] != null) this.Format = (string)settings["format"];
            if (settings["text_color"] != null) this.TextColor = (string)settings["text_color"];
            this.textBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(this.TextColor.ToUpper()));
            UpdateTime(); 
            this.timer = new Timer();
            this.timer.Interval = 1000;
            this.timer.Elapsed += delegate {
                UpdateTime();
            };
            this.timer.Start();
        }

        private void UpdateTime()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
            {
                this.textBlock.Text = DateTime.Now.ToString(Format);
            }));
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
            this.timer.Stop();
            this.timer.Dispose();
        }

        public FrameworkElement GetComponent()
        {
            return this;
        }
    }
}
