using AtarisUI.Utilities;
using NAudio.CoreAudioApi;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AtarisUI.Widgets
{
    /// <summary>
    /// Interaction logic for Audio.xaml
    /// </summary>
    public partial class Audio : UserControl, IWidget
    {
        private AudioDeviceNotificationClient client;
        private MMDeviceEnumerator enumerator;
        private MMDevice device;

        public Audio(dynamic settings)
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            VolumeSlider.ValueChanged += VolumeSlider_ValueChanged;
            VolumeSlider.PreviewMouseWheel += VolumeSlider_PreviewMouseWheel;

            client = new AudioDeviceNotificationClient();
            client.DefaultDeviceChanged += Client_DefaultDeviceChanged;

            enumerator = new MMDeviceEnumerator();
            /*foreach (var wasapi in enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
            {
                Console.WriteLine(wasapi.FriendlyName);
            }*/
            enumerator.RegisterEndpointNotificationCallback(client);

            if (enumerator.HasDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia))
            {
                UpdateDevice(enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia));
            } 

            this.Loaded += OnLoaded;
        }

        private void Client_DefaultDeviceChanged(object sender, string e)
        {
            MMDevice mmdevice = string.IsNullOrEmpty(e) ? null : enumerator.GetDevice(e);
            UpdateDevice(mmdevice);
        }

        private void UpdateDevice(MMDevice mmdevice)
        {
            if (device != null)
            {
                device.AudioEndpointVolume.OnVolumeNotification -= AudioEndpointVolume_OnVolumeNotification;
            }

            device = mmdevice;
            if (device != null)
            {
                try
                {
                    UpdateVolume(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
                    device.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
                }
                catch { }

                //Application.Current.Dispatcher.Invoke(() => PrimaryContent = volumeControl);
            }
            //else { Application.Current.Dispatcher.Invoke(() => PrimaryContent = noDeviceMessageBlock); }
        }

        private void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            UpdateVolume(data.MasterVolume * 100);
        }

        private bool _isInCodeValueChange;

        private void _SliderSetVolume(double value, RoutedEventArgs e)
        {
            if (device == null) return;
            try
            {
                device.AudioEndpointVolume.MasterVolumeLevelScalar = (float)(value / 100);
            }
            catch { } //99.9% is "A device attached to the system is not functioning" (0x8007001F), ignore this

            if (value == 0 && !device.AudioEndpointVolume.Mute)
                device.AudioEndpointVolume.Mute = true;
            else if (value > 0 && device.AudioEndpointVolume.Mute)
                device.AudioEndpointVolume.Mute = false;

            e.Handled = true;
        }

        private void UpdateVolume(double volume)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                UpdateVolumeGlyph(volume);
                this.textBlock.Text = "Volume: " + Math.Round(volume).ToString();
                _isInCodeValueChange = true;
                VolumeSlider.Value = volume;
                _isInCodeValueChange = false;
            });
        }

        private void UpdateVolumeGlyph(double volume)
        {
            if (device != null && !device.AudioEndpointVolume.Mute)
            {
                Icon.Visibility = Visibility.Visible;
                IconBackground.Source = getImage(@"..\Resources\volume_background.png");

                if (volume < 1)
                {
                    Icon.Visibility = Visibility.Hidden;
                    IconBackground.Source = getImage(@"..\Resources\volume_muted.png");
                } else if (volume < 33)
                {
                    Icon.Source = getImage(@"..\Resources\volume1.png");
                }
                else if (volume < 66)
                {
                    Icon.Source = getImage(@"..\Resources\volume2.png");
                }
                else
                {
                    Icon.Source = getImage(@"..\Resources\volume3.png");
                }
            }
            else
            {
                Icon.Visibility = Visibility.Hidden;
                IconBackground.Source = getImage(@"..\Resources\volume_muted.png");
            }
        }

        private ImageSource getImage(string uri)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri(uri, UriKind.Relative));
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            return bitmapImage;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_isInCodeValueChange)
            {
                var value = e.NewValue;
                var oldValue = e.OldValue;

                if (value == oldValue)
                {
                    return;
                }

                if (oldValue != value)
                {
                    _SliderSetVolume(value, e);
                }
            }
        }

        private void VolumeSlider_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var slider = sender as Slider;
            var change = e.Delta / 120.0;

            var volume = Math.Min(Math.Max(slider.Value + change, 0.0), 100.0);

            if (device != null)
            {
                try
                {
                    device.AudioEndpointVolume.MasterVolumeLevelScalar = (float)(volume / 100.0);
                }
                catch { }

                e.Handled = true;
            }
            _SliderSetVolume(volume, e);
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

        public void Button_Settings(object sender, MouseButtonEventArgs e)
        {
            Process.Start("ms-settings:sound");
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
