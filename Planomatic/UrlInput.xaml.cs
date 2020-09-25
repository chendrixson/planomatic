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
using System.Windows.Shapes;

namespace Planomatic
{
    /// <summary>
    /// Interaction logic for UrlInput.xaml
    /// </summary>
    public partial class UrlInput : Window
    {
        public Config myConfig()
        {
            return ((App)Application.Current).CurrentConfig;
        }

        public UrlInput()
        {
            InitializeComponent();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            // Get a file then load the config
            myConfig().LoadConfigFromUrl(Url.Text);
            this.Close();
        }
    }
}
