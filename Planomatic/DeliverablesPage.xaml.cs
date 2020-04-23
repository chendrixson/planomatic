using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Planomatic
{
    /// <summary>
    /// Interaction logic for DeliverablesPage.xaml
    /// </summary>
    public partial class DeliverablesPage : Page
    {
        private DispatcherTimer _searchTimer = new DispatcherTimer();

        public DeliverablesPage()
        {
            InitializeComponent();

            this.DataContext = myApp().DeliverableList;
            _searchTimer.Tick += new EventHandler(OnSearchTimerTick);
            _searchTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
        }

        public Config myConfig()
        {
            return ((App)Application.Current).CurrentConfig;
        }
        private App myApp()
        {
            return (App)App.Current;
        }

        public void RefreshView()
        {
            myApp().DeliverableList.Resort();
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshButton.IsEnabled = false;
            myConfig().RefreshingItems = true;

            await myApp().DeliverableList.Refresh();

            myConfig().RefreshingItems = false;
            RefreshButton.IsEnabled = true;

            SearchCountTextBlock.Text = myApp().DeliverableList.AllItems.Count.ToString();
        }

        private void Resort_Click(object sender, RoutedEventArgs e)
        {
            myApp().DeliverableList.Resort();
        }

        private async void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateButton.IsEnabled = false;

            await myApp().DeliverableList.Update();

            UpdateButton.IsEnabled = true;
        }

        private void FourSevensItem_Click(object sender, RoutedEventArgs e)
        {
            if (AllDeliverableList.SelectedIndex != -1)
            {
                myApp().DeliverableList.AllItems[AllDeliverableList.SelectedIndex].Rank = 7777;
                myApp().DeliverableList.AllItems[AllDeliverableList.SelectedIndex].Mod = true;
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RoutedEventArgs e)
        {
            if (AllDeliverableList.SelectedIndex != -1)
            {
                string url = myApp().DeliverableList.AllItems[AllDeliverableList.SelectedIndex].AdoUrl;
                myApp().UpdateStatus("Opening Url: " + url);

                System.Diagnostics.Process.Start(url);
            }
        }

        private void TeamsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox teamsComboBox = sender as ComboBox;
            Deliverable selectedDeliverableItem = this.AllDeliverableList.CurrentItem as Deliverable;
            string prevTeam = selectedDeliverableItem.Team;
            string newTeam = teamsComboBox.SelectedItem as string;

            // In case previous team = new team - no op - return.
            if (string.Equals(prevTeam, newTeam, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // In case <unmapped> was selected - don't allow it - return.
            if (string.Equals(newTeam, "<unmapped>", StringComparison.OrdinalIgnoreCase))
            {
                teamsComboBox.SelectedItem = prevTeam;
                return;
            }

            // We found a new team (previous could be either <unmapped> or a different team)
            // For building full area path - We assume that all teams are sub node of root node
            myApp().UpdateStatus($"Team Area path changed, was: {prevTeam}, now: {newTeam}");

            selectedDeliverableItem.Team = newTeam;
            selectedDeliverableItem.AreaPath = $@"{myConfig().TeamsRootNode}\{newTeam}";
            selectedDeliverableItem.AreaPathModified = true;
            selectedDeliverableItem.Mod = true;
        }

        private void ShowCustomString1_Checked(object sender, RoutedEventArgs e)
        {
            CustomString1Header.Visibility = Visibility.Visible;
        }

        private void ShowCustomString1_Unchecked(object sender, RoutedEventArgs e)
        {
            CustomString1Header.Visibility = Visibility.Hidden;
        }

        private void ShowCustomString2_Checked(object sender, RoutedEventArgs e)
        {
            CustomString2Header.Visibility = Visibility.Visible;
        }

        private void ShowCustomString2_Unchecked(object sender, RoutedEventArgs e)
        {
            CustomString2Header.Visibility = Visibility.Hidden;
        }

        private void ShowCustomString3_Checked(object sender, RoutedEventArgs e)
        {
            CustomString3Header.Visibility = Visibility.Visible;
        }

        private void ShowCustomString3_Unchecked(object sender, RoutedEventArgs e)
        {
            CustomString3Header.Visibility = Visibility.Hidden;
        }

        private void ShowPMOwner_Checked(object sender, RoutedEventArgs e)
        {
            PMOwnerHeader.Visibility = Visibility.Visible;
        }

        private void ShowPMOwner_Unchecked(object sender, RoutedEventArgs e)
        {
            PMOwnerHeader.Visibility = Visibility.Hidden;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private void OnSearchTimerTick(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            Search(SearchTextBox.Text);
        }

        private void Search(string searchText)
        {
            int showCount = 0;
            Task.Run(() =>
            {
                // Execute search on a non-UI thread
                foreach (Deliverable d in myApp().DeliverableList.AllItemsView)
                {
                    if (d.Title.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        d.IsHidden = false;
                        showCount++;
                    }
                    else
                    {
                        d.IsHidden = true;
                    }
                }

                Dispatcher.Invoke(() =>
                {
                    SearchCountTextBlock.Text = showCount.ToString();
                });
            });
        }
    }
}

