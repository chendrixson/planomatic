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

namespace Planomatic
{
    /// <summary>
    /// Interaction logic for CS1Page.xaml
    /// </summary>
    public partial class CS1Page : Page
    {
        public CS1Page()
        {
            InitializeComponent();

            AllGroupsList.DataContext = myApp().DeliverableList;
            CurrentGroupList.DataContext = myApp().DeliverableList;
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


        private void ItemPopup_Opened(object sender, RoutedEventArgs e)
        {
            // Make sure something is selected
            if(AllGroupsList.SelectedIndex == -1)
            {
                myApp().UpdateStatus("Select group to open");
                return;
            }

            // Adjust the popup
            ItemPopupGrid.Width = this.ActualWidth;

            var deliverableList = myApp().DeliverableList;
            var groupList = deliverableList.AllGroups;
            var sourceGroup = groupList[AllGroupsList.SelectedIndex];

            var currentGroupList = deliverableList.CurrentGroup;
            currentGroupList.Clear();

            foreach(Deliverable d in sourceGroup.GroupDeliverables)
            {
                currentGroupList.Add(d);
            }
        }

        private void CombineCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            myConfig().CombineCustomStringGrouping = true;
        }

        private void CombineCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            myConfig().CombineCustomStringGrouping = false;
        }
    }
}
