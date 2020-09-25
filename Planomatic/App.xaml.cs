using System;
using System.Windows;

namespace Planomatic
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Config CurrentConfig = new Config("placeholder");

        // Create deliverable list object with our config data to reference
        public Deliverables DeliverableList = new Deliverables();

        public void UpdateStatus(string status)
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            if (mainWindow != null)
            {
                mainWindow.UpdateStatus(status);
            }
        }

        public void UpdateProgress(bool state)
        {
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            if (mainWindow != null)
            {
                mainWindow.StatusProgress.IsIndeterminate = state;
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // If we are load with a .plano file - load config, otherwise load placeholder
            String[] arguments = Environment.GetCommandLineArgs();
            if (arguments.GetLength(0) > 1)
            {
                if (arguments[1].EndsWith(".plano"))
                {
                    string filePathFormMainArgs = arguments[1];
                    CurrentConfig.LoadConfigFromFile(filePathFormMainArgs);
                }
            }
        }
    }
}
