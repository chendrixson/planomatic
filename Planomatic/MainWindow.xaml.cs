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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Planomatic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ConfigPage _configPage;
        DeliverablesPage _deliverablePage;
        CS1Page _cs1Page;
        TeamViewPage _teamViewPage;
        ScaleTransform _zoomTransform = new ScaleTransform();

        public ConfigPage CurrentConfigPage
        {
            get { return _configPage; }
            set { }
        }

        public MainWindow()
        {
            _configPage = new ConfigPage();
            _configPage.LayoutTransform = _zoomTransform;

            myApp().DeliverableList.SetConfig(myApp().CurrentConfig);

            InitializeComponent();

            // grab the version
            
        }
        private App myApp()
        {
            return (App)App.Current;
        }

        private void TeamView_Click(object sender, RoutedEventArgs e)
        {
            if(_teamViewPage == null)
            {
                _teamViewPage = new TeamViewPage();

                _teamViewPage.LayoutTransform = _zoomTransform;
            }

            MainFrame.Content = _teamViewPage;
        }

        private void Config_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = _configPage;
        }

        public void UpdateStatus(string status)
        {
            StatusText.Content = status;
        }

        private void Deliverables_Click(object sender, RoutedEventArgs e)
        {
            if (_deliverablePage == null)
            {
                _deliverablePage = new DeliverablesPage();

                _deliverablePage.LayoutTransform = _zoomTransform;
            }

            MainFrame.Content = _deliverablePage;

            _deliverablePage.RefreshView();
        }

        private void CS1_Click(object sender, RoutedEventArgs e)
        {
            if (_cs1Page == null)
            {
                _cs1Page = new CS1Page();

                _cs1Page.LayoutTransform = _zoomTransform;
            }

            MainFrame.Content = _cs1Page;

            _cs1Page.RefreshView();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double scale = (double)ZoomSlider.Value / 100;

            _zoomTransform.ScaleX = scale;
            _zoomTransform.ScaleY = scale;
        }

        private void MenuPaneToggle_Click(object sender, RoutedEventArgs e)
        {

            // NON ANIMATED VERSIONT
            if (MenuPaneToggle.IsChecked == false)
            {
                // Shrinking
                MenuPaneToggle.HorizontalAlignment = HorizontalAlignment.Left;
                MenuPaneToggle.Margin = new Thickness(0); ;
                MenuColumn.Width = new GridLength(MenuPaneToggle.Width);
                MainFrameColumn.Width = new GridLength(1, GridUnitType.Star);
                ZoomSlider.Visibility = Visibility.Hidden;
                ZoomSliderLabel.Visibility = Visibility.Hidden;
            }
            else
            {
                // Expanding!
                MenuPaneToggle.HorizontalAlignment = HorizontalAlignment.Right;
                MenuPaneToggle.Margin = new Thickness(10); ;
                MenuColumn.Width = new GridLength(1, GridUnitType.Star);
                MainFrameColumn.Width = new GridLength(4, GridUnitType.Star);
                ZoomSlider.Visibility = Visibility.Visible;
                ZoomSliderLabel.Visibility = Visibility.Visible;
            }

        }
    }
}
