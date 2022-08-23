using AtarisUI.Controls;
using AtarisUI.Interop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace AtarisUI
{
    /// <summary>
    /// Interaction logic for TopBarWindow.xaml
    /// </summary>
    public partial class TopBarWindow : Window
    {
        public static TopBarWindow Instance { get; private set; }

        IntPtr handle;
        bool isHidden = false;
        public List<object> Uncollectable = new List<object>();
        public List<IWidget> Widgets = new List<IWidget>();
        public List<Assembly> Extentions = new List<Assembly>();
        public dynamic Config;
        private bool isBlured = false;
        private bool allowClosing = false;
        private WindowName windowName;

        public TopBarWindow(dynamic config)
        {
            // Prevent launching multiple instances
            Process current = Process.GetCurrentProcess();
            foreach (Process p in Process.GetProcesses())
            {
                //if (p.ProcessName.Equals(current.ProcessName) && p.StartInfo.FileName.Equals(current.StartInfo.FileName) && !p.Id.Equals(current.Id)) p.Kill();
            }
            // Enable visual styles for WinForms controls
            System.Windows.Forms.Application.EnableVisualStyles();
            // Initialize WPF components
            InitializeComponent();

            this.Config = config;
            if (Config["text_color"] == null) {
                Config["text_color"] = "#FFFFFF";
            }

            // Hide window from Alt+Tab
            this.HideWindow();
            // Saving handle
            this.handle = new WindowInteropHelper(this).Handle;
            // Subscribe events
            this.SetupEvents();
            // Setup timer
            Timer timer = new Timer();
            timer.Tick += Tick;
            timer.Interval = 10;
            timer.Start();
            Timer timerRgb = new Timer();
            timerRgb.Tick += RgbTick;
            timerRgb.Interval = 10;
            timerRgb.Start();
            // Reset before seutp
            this.Reset();
            // Run seutp
            this.Build();

            Instance = this;
        }

        private void HideWindow()
        {
            /*// Create helper window
            Window w = new Window();
            // Location of new window is outside of visible part of screen
            w.Top = -100;
            w.Left = -100;
            // Size of window is enough small to avoid its appearance at the beginning
            w.Width = 1;
            w.Height = 1;

            // Set window style as ToolWindow to avoid its icon in AltTab
            w.WindowStyle = WindowStyle.ToolWindow;
            w.ShowInTaskbar = false;
            w.Show();
            this.Owner = w;
            w.Hide();
            // Close helper window when main window is closed
            this.Closed += delegate { w.Close(); };
            base.Focusable = false;*/
        }

        private void UpdateCurrentWindowTitle()
        {
            if (windowName != null)
            {
                string title = WinApi.GetActiveWindowTitle();
                if (title == "TopBar") return;
                this.windowName.SetText(title.Length == 0 ? "Desktop" : title);
            }
        }

        private void SetupEvents()
        {
            this.Closing += delegate (object sender, CancelEventArgs e) {
                if (this.allowClosing)
                {
                    this.Reset();
                }
                else e.Cancel = true;
            };
            this.StateChanged += delegate { this.WindowState = WindowState.Normal; };
            WinApi.WinEventDelegate windowSwitched = new WinApi.WinEventDelegate(
                delegate (IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime) {
                    this.UpdateCurrentWindowTitle();
                }
            );
            Uncollectable.Add(windowSwitched);
            WinApi.SetWinEventHook(3, 3, IntPtr.Zero, windowSwitched, 0, 0, 0);
        }

        private void Build()
        {
            try
            {
                TopBarItem startMenu = new TopBarItem(new StartMenu());
                startMenu.Width = 40;
                this.LeftPanel.Children.Add(startMenu);
                
                this.windowName = new WindowName();
                TopBarItem windowNameItem = new TopBarItem(windowName);
                this.LeftPanel.Children.Add(windowNameItem);

                // Setup window
                this.WindowState = WindowState.Normal;
                this.Left = Screen.PrimaryScreen.WorkingArea.Left;
                this.Top = Screen.PrimaryScreen.WorkingArea.Top;
                this.Width = Screen.PrimaryScreen.WorkingArea.Width;
                this.Height = 24;
                // Use Windows API to create margin for bar
                RECT rect = new RECT();
                rect.Left = Screen.PrimaryScreen.WorkingArea.Left;
                rect.Top = Screen.PrimaryScreen.WorkingArea.Top;
                rect.Right = Screen.PrimaryScreen.WorkingArea.Right;
                rect.Bottom = Screen.PrimaryScreen.WorkingArea.Bottom;
                APPBARDATA appbardata = new APPBARDATA();
                appbardata.rect = rect;
                appbardata.cbSize = (uint) Marshal.SizeOf((object)appbardata);
                appbardata.hWnd = this.handle;
                appbardata.uEdge = AppBarEdge.Top;
                Shell32.SHAppBarMessage(AppBarMessage.New, ref appbardata);
                Shell32.SHAppBarMessage(AppBarMessage.QueryPos, ref appbardata);
                appbardata.rect.Bottom = checked(appbardata.rect.Top + Convert.ToInt32(24));
                Shell32.SHAppBarMessage(AppBarMessage.SetPos, ref appbardata);

                // Load extentions
                foreach (string dll in Directory.GetFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AtarisUI", "extensions"), "*.dll"))
                    this.Extentions.Add(Assembly.LoadFile(dll));

                // Enable blur
                if (!this.isBlured && (bool)this.Config["blur"])
                {
                    this.isBlured = true;
                    WindowBlur.SetIsEnabled(this, true);
                }

                // Set backgroud
                if ((bool)this.Config["rgb"])
                {
                    this.Background = new SolidColorBrush(Color.FromArgb(
                        (byte)this.Config.background_color.alpha,
                        255,
                        0,
                        0
                        ));
                }
                else
                {
                    this.Background = new SolidColorBrush(Color.FromArgb(
                        (byte)this.Config.background_color.alpha,
                        (byte)this.Config.background_color.red,
                        (byte)this.Config.background_color.green,
                        (byte)this.Config.background_color.blue
                        ));
                }

                // Setup focused window title styles
                this.windowName.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)this.Config["text_color"]));
                this.windowName.Width = this.Width / 2;

                // Setup widgets
                try
                {
                    this.SetupWidgets(this.Config);
                }
                catch (Exception err)
                {
                    System.Windows.MessageBox.Show(err.ToString(), "Failed to setup widgets!");
                    throw new Exception("");
                }
            }
            catch (Exception err)
            {
                if (err.Message.Length > 0)
                {
                    System.Windows.MessageBox.Show(err.ToString(), "Failed to load TopBar!");
                }
            }
        }

        private void SetupWidgets(dynamic config)
        {
            foreach (dynamic widget in config.widgets)
            {
                Type widgetType = Type.GetType((string)widget.type);
                if (widgetType == null)
                {
                    foreach (Assembly extention in this.Extentions)
                    {
                        widgetType = extention.GetType((string)widget.type);
                        if (widgetType != null) break;
                    }
                }
                if (widgetType == null)
                {
                    System.Windows.MessageBox.Show($"Can't find \"{(string)widget.type}\" widget!", "Error");
                    continue;
                };

                dynamic settings = widget.settings == null ? new object() : widget.settings;
                if (settings["text_color"] == null)
                {
                    settings["text_color"] = config["text_color"];
                }

                IWidget widgetObject = (IWidget)widgetType.GetConstructor(new Type[] { typeof(object) }).Invoke(new object[] { settings });

                TopBarItem topBarItem = new TopBarItem(widgetObject);
                this.WidgetsPanel.Children.Add(topBarItem);
                this.Widgets.Add(widgetObject);
            }
        }

        private void Reset()
        {
            APPBARDATA appbardata = new APPBARDATA();
            appbardata.cbSize = (uint) Marshal.SizeOf((object)appbardata);
            appbardata.hWnd = this.handle;
            Shell32.SHAppBarMessage(AppBarMessage.Remove, ref appbardata);
             
            foreach (IWidget widget in Widgets)
            {
                widget.Dispose();
            }
            this.Widgets.Clear();
            this.WidgetsPanel.Children.Clear();

            this.Extentions.Clear();
        }

        private void RgbTick(object sender, EventArgs e)
        {
            if ((bool)this.Config["rgb"])
            {
                SolidColorBrush oldBrush = (SolidColorBrush)this.Background;

                byte r = oldBrush.Color.R;
                byte g = oldBrush.Color.G;
                byte b = oldBrush.Color.B;

                if (r > 0 && b == 0)
                {
                    r--;
                    g++;
                }
                if (g > 0 && r == 0)
                {
                    g--;
                    b++;
                }
                if (b > 0 && g == 0)
                {
                    b--;
                    r++;
                }

                this.Background = new SolidColorBrush(Color.FromArgb(oldBrush.Color.A, r, g, b));
            }
        }

        private void Tick(object sender, EventArgs e)
        {
            this.UpdateCurrentWindowTitle();
            if (WinApi.IsForegroundFullScreen(Screen.PrimaryScreen))
            {
                if (!isHidden)
                {
                    isHidden = true;
                    this.Hide();
                }
            }
            else
            {
                if (isHidden)
                {
                    isHidden = false;
                    this.Show();
                }
            }
        }

        public void Reload()
        {
            Close();
            System.Windows.Forms.Application.Restart();
        }

        public new void Close()
        {
            this.allowClosing = true;
            base.Close();
        }

        private void ContextMenu_ShowConfigTB(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", "/select, \"" + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "AtarisUI", "config.json") + "\"");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);

            int exStyle = (int)User32.GetWindowLong(wndHelper.Handle, (int)User32.GetWindowLongFields.GWL_EXSTYLE);

            //exStyle = exStyle | User32.WS_EX_NOACTIVATE;
            exStyle = exStyle | User32.WS_EX_TOOLWINDOW;
            User32.SetWindowLong(wndHelper.Handle, (int)User32.GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);

            /*User32.SetWindowRgn(wndHelper.Handle, Gdi32.CreateRoundRectRgn(0, 0, 1920, 24, 20, 20), true);

            var blur = new DwmApi.DWM_BLURBEHIND
            {
                dwFlags = DwmApi.DWM_BB.DWM_BB_ENABLE | DwmApi.DWM_BB.DWM_BB_BLURREGION | DwmApi.DWM_BB.DWM_BB_TRANSITIONONMAXIMIZED,
                fEnable = true,
                hRgnBlur = (IntPtr) null,
                fTransitionOnMaximized = true
            };

            DwmApi.DwmEnableBlurBehindWindow(wndHelper.Handle, ref blur);*/
        }
    }
}
