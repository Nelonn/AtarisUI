using System;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace AtarisUI.Widgets
{
    class TextFromFile : IWidget
    {
        private string MainColor = "#BBBBBB";
        private string FilePath = "";
        private TextBlock textBlock;
        private Timer timer;

        public TextFromFile(dynamic settings)
        {
            if (settings["color"] != null) this.MainColor = (string)settings["color"];
            if (settings["file"] != null) this.FilePath = (string)settings["file"];
            this.textBlock = new TextBlock();
            this.textBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(this.MainColor));
            this.timer = new Timer();
            this.timer.Interval = 500;
            this.timer.Elapsed += delegate {
                string toRender = "";
                try
                {
                    toRender = File.ReadAllText(FilePath);
                    if (toRender.Length > 512) toRender = "[Error: Text is too long]";
                }
                catch (Exception err)
                {
                    toRender = $"[Error: {err.Message}]";
                }
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                {
                    this.textBlock.Text = toRender;
                }));
            };
            this.timer.Start();
        }

        public void Dispose()
        {
            this.timer.Stop();
            this.timer.Dispose();
        }

        public void MouseLeftButtonDown()
        {

        }

        public void MouseLeftButtonUp()
        {

        }

        public void MouseRightButtonDown()
        {

        }

        public void MouseRightButtonUp()
        {

        }

        public FrameworkElement GetComponent()
        {
            return this.textBlock;
        }
    }
}
