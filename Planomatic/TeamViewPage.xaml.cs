using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Planomatic
{
    /// <summary>
    /// Interaction logic for TeamViewPage.xaml
    /// </summary>
    public partial class TeamViewPage : Page
    {
        public TeamViewPage()
        {
            InitializeComponent();

            DataContext = myConfig();
        }

        public Config myConfig()
        {
            return ((App)Application.Current).CurrentConfig;
        }
        private App myApp()
        {
            return (App)App.Current;
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshButton.IsEnabled = false;
            myConfig().RefreshingItems = true;

            await myApp().DeliverableList.Refresh();

            myConfig().RefreshingItems = false;
            RefreshButton.IsEnabled = true;
        }

        private void RadioButton_Changed(object sender, System.Windows.RoutedEventArgs e)
        {
            // Update the team sum config
            if(SumModeAll.IsChecked == true)
            {
                myConfig().CurrentSumMode = Config.TeamSumMode.AllItems;
            }
            else if(SumModeAboveCut.IsChecked == true)
            {
                myConfig().CurrentSumMode = Config.TeamSumMode.AboveCutLine;
            }
            else
            {
                myConfig().CurrentSumMode = Config.TeamSumMode.AboveRank;
            }

            myApp().DeliverableList.RecalcSums();
        }
    }
}
