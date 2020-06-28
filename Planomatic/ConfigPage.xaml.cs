using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using SWF = System.Windows.Forms;

namespace Planomatic
{
    /// <summary>
    /// Interaction logic for ConfigPage.xaml
    /// </summary>
    public partial class ConfigPage : Page
    {
        public Config myConfig()
        {
            return ((App)Application.Current).CurrentConfig;
        }
        public ConfigPage()
        {
            InitializeComponent();

            DataContext = myConfig();

            myConfig().IterationListChanged += ConfigPage_IterationListChanged;

            // Call it once to update the headers
            ConfigPage_IterationListChanged(myConfig().IterationList);

        }
        

        private void ConfigPage_IterationListChanged(string iterationList)
        {
            // Go through and update the headers for the team and day breakdown
            string[] iterations = iterationList.Split(';');

            ItHeader1.Header = "<>";
            ItHeader2.Header = "<>";
            ItHeader3.Header = "<>";
            ItHeader4.Header = "<>";
            ItHeader5.Header = "<>";
            ItHeader6.Header = "<>";

            DayHeader1.Header = "<>";
            DayHeader2.Header = "<>";
            DayHeader3.Header = "<>";
            DayHeader4.Header = "<>";
            DayHeader5.Header = "<>";
            DayHeader6.Header = "<>";


            if (iterations.Length > 0)
            {
                ItHeader1.Header = iterations[0];
                DayHeader1.Header = iterations[0];
            }

            if (iterations.Length > 1)
            {
                ItHeader2.Header = iterations[1];
                DayHeader2.Header = iterations[1];
            }

            if (iterations.Length > 2)
            {
                ItHeader3.Header = iterations[2];
                DayHeader3.Header = iterations[2];
            }

            if (iterations.Length > 3)
            {
                ItHeader4.Header = iterations[3];
                DayHeader4.Header = iterations[3];
            }

            if (iterations.Length > 4)
            {
                ItHeader5.Header = iterations[4];
                DayHeader5.Header = iterations[4];
            }

            if (iterations.Length > 5)
            {
                ItHeader6.Header = iterations[5];
                DayHeader6.Header = iterations[5];
            }

            myConfig().UpdateCapacity();
        }

        // TEAM List Button Handlers
        private void DeleteTeam_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Got Delete on Selected Index " + TeamList.SelectedIndex);
            if (TeamList.SelectedIndex >= 0)
            {
                myConfig().Teams.RemoveAt(TeamList.SelectedIndex);
                myConfig().UpdateCapacity();
            }
        }

        private void NewTeam_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Adding another empty team");
            myConfig().AddTeam();
            myConfig().UpdateCapacity();
        }


        // CONFIG Save/Load Button Handlers
        private void SaveAs_Button_Click(object sender, RoutedEventArgs e)
        {
            // Use the File dialog to get a path
            var fileDialog = new SWF.SaveFileDialog();
            fileDialog.Filter = "Plan-O-Matic Files (*.plano)|*.plano";
            fileDialog.FilterIndex = 1;
            fileDialog.OverwritePrompt = false;

            var result = fileDialog.ShowDialog();
            switch(result)
            {
                case SWF.DialogResult.OK:
                    myConfig().SaveAs(fileDialog.FileName);
                    break;

                default:
                    break;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            myConfig().Save();
        }

        private async void LoadFrom_Click(object sender, RoutedEventArgs e)
        {
            // Use the File dialog to get a path
            var fileDialog = new SWF.OpenFileDialog();
            fileDialog.Filter = "Plan-O-Matic Files (*.plano)|*.plano";
            fileDialog.FilterIndex = 1;

            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case SWF.DialogResult.OK:
                    myConfig().LoadConfig(fileDialog.FileName);
                    await ((App)App.Current).DeliverableList.Refresh();
                    break;

                default:
                    break;
            }

        }
    }
}
