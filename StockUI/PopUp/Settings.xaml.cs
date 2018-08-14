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

namespace StockUI.PopUp
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public string APIKey
        {
            get
            {
                if (APIKeyTextBox == null) return string.Empty;
                return APIKeyTextBox.Text;
            }
        }

        public string MinimumGain
        {
            get
            {
                if (MinimumGainTextBox == null) return string.Empty;
                return MinimumGainTextBox.Text;
            }
        }

        public string MinimumDelay
        {
            get
            {
                if (MinimumDelayTextBox == null) return string.Empty;
                return MinimumDelayTextBox.Text;
            }
        }

        public string MinimumGrowth
        {
            get
            {
                if (MinimumGrowthTextBox == null) return string.Empty;
                return MinimumGrowthTextBox.Text;
            }
        }

        public string MinimumPrice
        {
            get
            {
                if (MinimumPriceTextBox == null) return string.Empty;
                return MinimumPriceTextBox.Text;
            }
        }

        public Settings()
        {
            InitializeComponent();
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

