using Stock_Data;
using System.Windows;
using System.Windows.Controls;

namespace StockUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StockDataRepository stock = new StockDataRepository();
        private string month;
        private bool HTMLGathered = false;
        private bool FundamentalsGathered = false;


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
                SizeToContent = SizeToContent.Width; //fixes poor sizing
                HTMLGathered = true;
            }           
        }

        private void AddFundamentals(object sender, RoutedEventArgs e)
        {   
            if (month != null && HTMLGathered)
            {
                var stockData = stock.RetrieveFundamentals(month); //sends month and adds fundamental data
                Data.ItemsSource = null; 
                Data.ItemsSource = stockData; //along with null set will refresh data
                SizeToContent = SizeToContent.Width; //fixes poor sizing
                FundamentalsGathered = true;
            }          
        }

        private void GatherFundamentals(object sender, RoutedEventArgs e)
        {
            if (month != null && HTMLGathered)
            {
                var stockData = stock.GatherFundamentals(month); //sends month and gathers fundamental data
                Data.ItemsSource = null;
                Data.ItemsSource = stockData; //along with null set will refresh data
                SizeToContent = SizeToContent.Width; //fixes poor sizing
                FundamentalsGathered = true;
            }
        }

        private void SelectMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            month = SelectMonth.SelectedItem.ToString();
            Title = month; //for debugging purposes
            HTMLGathered = false;
            FundamentalsGathered = false;
        }

        private void Compare(object sender, RoutedEventArgs e)
        {
            if (month != null && HTMLGathered && FundamentalsGathered)
            {
                var stockData = stock.GetComparisons(month);
                Data.ItemsSource = null;
                Data.ItemsSource = stockData; //along with null set will refresh data
                SizeToContent = SizeToContent.Width; //fixes poor sizing
            }
        }
    }
}
