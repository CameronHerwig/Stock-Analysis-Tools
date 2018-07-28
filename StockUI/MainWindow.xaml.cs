using Stock_Data;
using System.Collections.Generic;
using System.Linq;
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
        private string predictionMonth;
        private bool HTMLGathered = false;
        private bool FundamentalsGathered = false;


        public MainWindow()
        {
            InitializeComponent();
            SelectMonth.ItemsSource = stock.GetMonths();
            SelectPredictionMonth.ItemsSource = stock.GetMonths();
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

        private void SelectPredictionMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            predictionMonth = SelectPredictionMonth.SelectedItem.ToString();
            Title = predictionMonth; //for debugging purposes
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

        private void GetComparisons(object sender, RoutedEventArgs e)
        {
            var fundChooser = new FundamentalChooser();
            var testData = fundChooser.RetrieveAllComparisons();
            Data1.ItemsSource = testData;
            SizeToContent = SizeToContent.Width; //fixes poor sizing
        }

        private void Predict(object sender, RoutedEventArgs e)
        {
            if (predictionMonth != null)
            {
                var stockRepo = new StockDataRepository();
                var stockData = stockRepo.GatherFutureFundamentals(predictionMonth);

                if (ADX.Text != "ADX")
                {
                    stockData.RemoveAll(symbol => symbol.ADX > double.Parse(ADX.Text));
                }
                if (BBANDS.Text != "BBANDS")
                {
                    stockData.RemoveAll(symbol => symbol.BBANDS > double.Parse(BBANDS.Text));
                }
                if (BOP.Text != "BOP")
                {
                    stockData.RemoveAll(symbol => symbol.BOP > double.Parse(BOP.Text));
                }
                if (MACD.Text != "MACD")
                {
                    stockData.RemoveAll(symbol => symbol.MACD > double.Parse(MACD.Text));
                }
                if (MOM.Text != "MOM")
                {
                    stockData.RemoveAll(symbol => symbol.MOM > double.Parse(MOM.Text));
                }
                if (RSI.Text != "RSI")
                {
                    stockData.RemoveAll(symbol => symbol.RSI > double.Parse(RSI.Text));
                }

                PredictionData.ItemsSource = stockData;
                SizeToContent = SizeToContent.Width;
            }         
        }
    }
}
