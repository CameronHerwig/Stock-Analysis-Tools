using System.Windows;
using System.Windows.Controls;
using Stock_Data;

namespace StockUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StockDataRepository stock = new StockDataRepository();
        private string month;

        public MainWindow()
        {
            InitializeComponent();
            SelectMonth.ItemsSource = stock.GetMonths();
        }

        private void ExtractHTML(object sender, RoutedEventArgs e)
        {
            if(month != null)
            {
                var stockData = stock.RetrieveHTML(month); //sends month to get HTMl data
                Data.ItemsSource = stockData;
                this.SizeToContent = SizeToContent.Width; //fixes poor sizing
            }           
        }

        private void AddFundamentals(object sender, RoutedEventArgs e)
        {
            
            if (month != null)
            {
                var stockData = stock.RetrieveFundamentals(month); //sends month and adds fundamental data
                Data.ItemsSource = null; 
                Data.ItemsSource = stockData; //along with null set will refresh data
                this.SizeToContent = SizeToContent.Width; //fixes poor sizing
            }          
        }

        private void GatherFundamentals(object sender, RoutedEventArgs e)
        {
            if (month != null)
            {
                var stockData = stock.GatherFundamentals(month);
            }
        }

        private void SelectMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            month = SelectMonth.SelectedItem.ToString();
            Title = month; //for debugging purposes
        }
        
    }
}
