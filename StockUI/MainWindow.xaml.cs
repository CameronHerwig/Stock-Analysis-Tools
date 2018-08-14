using Stock_Data;
using StockData.Fundamentals;
using StockUI.PopUp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace StockUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        StockDataRepository stock = new StockDataRepository();
        StockDataRepository stockRepo = new StockDataRepository();
        DateRepository dateRepo = new DateRepository();
        private string month;
        private string predictionMonth;
        private bool HTMLGathered = false;
        private bool predictHTMLGathered = false;
        private bool FundamentalsGathered = false;
        private bool Ready = true;
        private bool showErrors = false;
        private bool keepEmpties = false;
        private bool datesGathered = false;
        private bool showPredictErrors = false;

        public MainWindow()
        {
            InitializeComponent();
            SelectMonth.ItemsSource = stock.GetMonths();
            SelectPredictionMonth.ItemsSource = stock.GetMonths();
        }

        private void ExtractHTML(object sender, RoutedEventArgs e)
        {
            if(month != null && Ready)
            {
                Ready = false;
                var stockData = stock.RetrieveHTML(month); //sends month to get HTMl data
                Data.ItemsSource = stockData;
                SizeToContent = SizeToContent.Width; //fixes poor sizing
                HTMLGathered = true;
                Ready = true;
            }           
        }

        private void AddFundamentals(object sender, RoutedEventArgs e)
        {   
            if (month != null && HTMLGathered && Ready)
            {
                Ready = false;
                var stockData = stock.RetrieveFundamentals(month); //sends month and adds fundamental data
                if(stockData.Count != 0)
                {
                    Data.ItemsSource = null;
                    Data.ItemsSource = stockData; //along with null set will refresh data
                }               
                SizeToContent = SizeToContent.Width; //fixes poor sizing
                FundamentalsGathered = true;
                Ready = true;
            }          
        }

        private async void GatherFundamentals(object sender, RoutedEventArgs e)
        {
            if (month != null && HTMLGathered && Ready && datesGathered)
            {
                Ready = false;
                Task.Run(() => StartTimer());
                Title = "Gathering Fundamentals - Please Wait";
                var stockData = await Task.Run(() => stock.GatherFundamentals(month, showErrors)); //sends month and gathers fundamental data
                Data.ItemsSource = null;
                Data.ItemsSource = stockData; //along with null set will refresh data
                SizeToContent = SizeToContent.Width; //fixes poor sizing
                FundamentalsGathered = true;
                Ready = true;
            }
        }

        private void Compare(object sender, RoutedEventArgs e)
        {
            if (month != null && HTMLGathered && FundamentalsGathered && Ready)
            {
                Ready = false;
                var stockData = stock.GetComparisons(month);
                Data.ItemsSource = null;
                Data.ItemsSource = stockData; //along with null set will refresh data
                SizeToContent = SizeToContent.Width; //fixes poor sizing
                Ready = true;
            }
        }

        private void GetComparisons(object sender, RoutedEventArgs e)
        {
            var fundChooser = new FundamentalChooser();
            Data1.Columns.Clear();
            Data1.ItemsSource = null;
            Data1.ItemsSource = fundChooser.RetrieveAllComparisonsDynamic(keepEmpties);
            var rows = Data1.ItemsSource.OfType<IDictionary<string, object>>();
            var columns = rows.SelectMany(d => d.Keys).Distinct(StringComparer.OrdinalIgnoreCase);

            foreach (string text in columns)
            {
                // now set up a column and binding for each property
                var column = new DataGridTextColumn
                {
                    Header = text,
                    Binding = new Binding(text)
                };

                Data1.Columns.Add(column);
            }
            SizeToContent = SizeToContent.Width; //fixes poor sizing
        }

        private async void Predict(object sender, RoutedEventArgs e)
        {
            if (predictionMonth != null && Ready && predictHTMLGathered && datesGathered)
            {
                Ready = false;
                Title = "Gathering Fundamentals - Please Wait";
                Task.Run(() => StartTimer());
                var stockData = await Task.Run(() => stockRepo.GatherFutureFundamentals(predictionMonth, showErrors));
                PredictionData.ItemsSource = null;
                PredictionData.ItemsSource = stockData;
                if (ADX.Text != "")
                {
                    stockData.RemoveAll(symbol => symbol.ADX > double.Parse(ADX.Text));
                }
                if (BBANDS.Text != "")
                {
                    stockData.RemoveAll(symbol => symbol.BBANDS > double.Parse(BBANDS.Text));
                }
                if (BOP.Text != "")
                {
                    stockData.RemoveAll(symbol => symbol.BOP > double.Parse(BOP.Text));
                }
                if (MACD.Text != "")
                {
                    stockData.RemoveAll(symbol => symbol.MACD > double.Parse(MACD.Text));
                }
                if (MOM.Text != "")
                {
                    stockData.RemoveAll(symbol => symbol.MOM > double.Parse(MOM.Text));
                }
                if (RSI.Text != "")
                {
                    stockData.RemoveAll(symbol => symbol.RSI > double.Parse(RSI.Text));
                }

                PredictionData.ItemsSource = stockData;
                SizeToContent = SizeToContent.Width;
                Ready = true;
            }         
        }

        public async Task StartTimer()
        {
            await Task.Run(async () =>
            {
            while (Ready == false)
                {
                    GetProgress();
                    await Task.Delay(15000);
                }
            });
            Title = "Fundamentals Gathered";
        }

        private void GetProgress()
        {
            var stockCount = StockDataRepository.stockCount;
            var fundCount = StockDataRepository.fundCount;
            this.Dispatcher.Invoke(() =>
            {
                Title = $"Gathering Fundamentals - Please Wait - {fundCount}/{stockCount}";
            });
        }

        private void PredictGetHTML(object sender, RoutedEventArgs e)
        {
            if (predictionMonth != null && Ready)
            {
                Ready = false;
                stockRepo = new StockDataRepository();
                var stockData = stockRepo.RetrieveFutureHTML(predictionMonth);
                PredictionData.ItemsSource = null;
                PredictionData.ItemsSource = stockData;
                SizeToContent = SizeToContent.Width;
                predictHTMLGathered = true;
                Ready = true;
            }
        }

        private async void GetDates(object sender, RoutedEventArgs e)
        {
            Title = "Gathering Dates - In Progress";
            await Task.Run(() => BuildDate());
            Title = "Gathering Dates - Complete";
        }

        private void BuildDate()
        {         
            dateRepo.GetSearchMonths();
            dateRepo.GetADX();
            dateRepo.GetBBANDS();
            dateRepo.GetBOP();
            dateRepo.GetMACD();
            dateRepo.GetMOM();
            dateRepo.GetRSI();
            dateRepo.GetPrice();
            dateRepo.GetGain();
            datesGathered = true;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            showErrors = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            showErrors = false;
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            keepEmpties = true;
        }
        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            keepEmpties = false;
        }

        private void PredictCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            showPredictErrors = true;
        }

        private void PredictCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            showPredictErrors = false;
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

        private void Help(object sender, RoutedEventArgs e)
        {
             HelpScreen popup = new HelpScreen();
             popup.ShowDialog();
        }

        private void Settings(object sender, RoutedEventArgs e)
        {
            Settings popup = new Settings();
            popup.ShowDialog();
            string APIKey = popup.APIKey;
            string MinimumGain = popup.MinimumGain;
            string MinimumDelay = popup.MinimumDelay;
            string MinimumGrowth = popup.MinimumGrowth;
            string MinimumPrice = popup.MinimumPrice;

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if(APIKey != String.Empty)
            {
                config.AppSettings.Settings.Remove("APIKey");
                config.AppSettings.Settings.Add("APIKey", APIKey);
            }
            if (MinimumGain != String.Empty)
            {
                config.AppSettings.Settings.Remove("MinimumGain");
                config.AppSettings.Settings.Add("MinimumGain", MinimumGain);
            }
            if (MinimumDelay != String.Empty)
            {
                config.AppSettings.Settings.Remove("MinimumDelay");
                config.AppSettings.Settings.Add("MinimumDelay", MinimumDelay);
            }
            if (MinimumGrowth != String.Empty)
            {
                config.AppSettings.Settings.Remove("MinimumGrowth");
                config.AppSettings.Settings.Add("MinimumGrowth", MinimumGrowth);
            }
            if (MinimumPrice != String.Empty)
            {
                config.AppSettings.Settings.Remove("MinimumPrice");
                config.AppSettings.Settings.Add("MinimumPrice", MinimumPrice);
            }
            
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
