using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
        TimeForBreakPage _timeForBreakViewPage;
        Page _lastVisitedPage;
        bool _timeForBreakIsOn = false;
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

            InitializeComponent();

            myApp().DeliverableList.SetConfig(myApp().CurrentConfig);
            myApp().DeliverableList.Refresh();

            // Load config page on startup
            MainFrame.Content = _configPage;

            // grab the version, if we're packaged up
            try
            {
                Windows.ApplicationModel.Package pkg = Windows.ApplicationModel.Package.Current;
                var v = pkg.Id.Version;
                VersionLabel.Content = "ver: " + v.Major + "." + v.Minor + "." + v.Build;
            }
            catch(System.Exception)
            {
                VersionLabel.Content = "Unpackaged";
            }
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

        private void TimeForBreak_Click(object sender, RoutedEventArgs e)
        {
            if (_timeForBreakIsOn)
            {
                TimeForBreakPageUnloadedCallback();
            }
            else
            {
                if (_timeForBreakViewPage == null)
                {
                    _timeForBreakViewPage = new TimeForBreakPage(TimeForBreakPageUnloadedCallback);
                }

                _lastVisitedPage = MainFrame.Content as Page;
                MainFrame.Content = _timeForBreakViewPage;
                TimeForBreakButton.Content = "Exit Time For Break";
                _timeForBreakIsOn = true;
            }
        }

        private void TimeForBreakPageUnloadedCallback()
        {
            if (_timeForBreakIsOn)
            {
                MainFrame.Content = _lastVisitedPage;
                TimeForBreakButton.Content = "Time For Break";
                _timeForBreakIsOn = false;
            }
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
                VersionLabel.Visibility = Visibility.Hidden;
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
                VersionLabel.Visibility = Visibility.Visible;
            }

        }
    }
}
