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
using System.ComponentModel;
using MaterialDesignThemes.Wpf;
using System.Diagnostics;

namespace Planomatic
{

    /// <summary>
    /// Interaction logic for DeliverablesPage.xaml
    /// </summary>
    public partial class DeliverablesPage : Page
    {        
        public DeliverablesPage()
        {
            InitializeComponent();

            AllDeliverableList.DataContext = myApp().DeliverableList;
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

        private void OpenItem_Click(object sender, RoutedEventArgs e)
        {
            if(AllDeliverableList.SelectedIndex != -1)
            {
                string url = myApp().DeliverableList.AllItems[AllDeliverableList.SelectedIndex].AdoUrl;
                myApp().UpdateStatus("Opening Url: " + url);

                System.Diagnostics.Process.Start(url);
            }
            else
            {
               
            }
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
    }
}

