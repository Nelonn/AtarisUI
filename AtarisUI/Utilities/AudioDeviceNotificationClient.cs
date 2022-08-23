using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using System;

namespace AtarisUI.Utilities
{
    public class AudioDeviceNotificationClient : IMMNotificationClient
    {
        public event EventHandler<string> DefaultDeviceChanged;

        public void OnDefaultDeviceChanged(DataFlow dataFlow, Role deviceRole, string defaultDeviceId)
        {
            if (dataFlow == DataFlow.Render && deviceRole == Role.Multimedia)
            {
                DefaultDeviceChanged?.Invoke(this, defaultDeviceId);
            }
        }

        public event EventHandler<string> DeviceAdded;

        public void OnDeviceAdded(string deviceId)
        {
            DeviceAdded?.Invoke(this, deviceId);
        }

        public event EventHandler<string> DeviceRemoved;

        public void OnDeviceRemoved(string deviceId)
        {
            DeviceRemoved?.Invoke(this, deviceId);
        }

        public void OnDeviceStateChanged(string deviceId, DeviceState newState)
        { }

        public void OnPropertyValueChanged(string deviceId, PropertyKey propertyKey)
        { }
    }
}
