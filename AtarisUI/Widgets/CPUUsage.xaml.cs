using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AtarisUI.Widgets
{
    /// <summary>
    /// Interaction logic for CPUUsage.xaml
    /// </summary>
    public partial class CPUUsage : UserControl, IWidget
    {
        private PerformanceCounter counter;
        private Timer timer;

        public CPUUsage(dynamic settings)
        {
            InitializeComponent();

            this.counter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            this.timer = new Timer();
            this.timer.Interval = 1000;
            this.timer.Elapsed += delegate 
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate
                {
                    textBlock.Text = $"CPU: {Math.Round(counter.NextValue())}%";
                }));
            };
            this.timer.Start();
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
