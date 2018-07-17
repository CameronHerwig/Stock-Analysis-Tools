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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(month != null)
            {
                var stockData = stock.RetrieveHTML(month); //sends month to get HTMl data
                Data.ItemsSource = stockData;
                this.SizeToContent = SizeToContent.Width; //fixes poor sizing
            }           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            if (month != null)
            {
                var stockData = stock.RetrieveFundamentals(month); //sends month and adds fundamental data
                Data.ItemsSource = null; 
                Data.ItemsSource = stockData; //along with null set will refresh data
                this.SizeToContent = SizeToContent.Width; //fixes poor sizing
            }          
        }

        private void SelectMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            month = ((ComboBoxItem)SelectMonth.SelectedItem).Content.ToString();
            Title = month; //for debugging purposes
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            //This is here so the compiler doesnt yell at me
        }
    }
}
