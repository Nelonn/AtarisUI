using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace AtarisUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string Version
        {
            get => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public dynamic config;

        public App()
        {
            //InitializeComponent();
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                config = ReadConfig();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Failed to load config!");
                throw err;
            }

            /*SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();*/

            TopBarWindow topBarWindow = new TopBarWindow(config["topbar"]);
            topBarWindow.Show();

            /*DockWindow dockWindow = new DockWindow(config["dock"]);
            dockWindow.Show();*/
        }

        private dynamic ReadConfig()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appfolder = Path.Combine(appdata, Assembly.GetEntryAssembly().GetName().Name);
            if (!Directory.Exists(appfolder)) Directory.CreateDirectory(appfolder);
            string extensions = Path.Combine(appfolder, "extensions");
            if (!Directory.Exists(extensions)) Directory.CreateDirectory(extensions);
            string configfile = Path.Combine(appfolder, "config.json");
            if (!File.Exists(configfile)) File.WriteAllBytes(configfile, AtarisUI.Properties.Resources.config);
            dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(configfile));

            if ((int)json["config-version"] != 1)
            {
                throw new UnsupportedConfigVersionException((int)json["config-version"]);
            }

            return json;
        }
    }
}
