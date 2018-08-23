using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using StockData.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StockData.Fundamentals
{
    public class DateRepository
    {
        private readonly int delay = int.Parse(Settings.Default.APIDelay); //Delay as mandated by API
        RestClient client = new RestClient("https://www.alphavantage.co/"); //Preps client for calls
        private readonly string apiKey = Settings.Default.APIKey; //Change as needed

        private static Dictionary<string, string> searchList = new Dictionary<string, string>();

        private static Dictionary<string, string> Months = new Dictionary<string, string> {
            {"Janurary", "01"},
            {"Feburary", "02"},
            {"March", "03"},
            {"April", "04"},
            {"May", "05"},
            {"June", "06"},
            {"July", "07"},
            {"August", "08"},
            {"September", "09"},
            {"October", "10"},
            {"November", "11"},
            {"December", "12"},
        };
        private static Dictionary<string, string> ADX = new Dictionary<string, string> { };
        private static Dictionary<string, string> BBANDS = new Dictionary<string, string> { };
        private static Dictionary<string, string> BOP = new Dictionary<string, string> { };
        private static Dictionary<string, List<string>> MOM = new Dictionary<string, List<string>> { };
        private static Dictionary<string, string> MACD = new Dictionary<string, string> { };
        private static Dictionary<string, string> RSI = new Dictionary<string, string> { };
        private static Dictionary<string, string> Price = new Dictionary<string, string> { };
        private static Dictionary<string, List<string>> Gain = new Dictionary<string, List<string>> { };

        public void GetADX()
        {
            var request = new RestRequest($"query?function=ADX&symbol=AAPL&interval=weekly&time_period=10&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            try //Should only catch API errors, otherwise will return as normal
            {
                var result = json["Technical Analysis: ADX"].Children().ToList();
                var testlist = new List<string>();
                foreach (var jobj in result)
                {
                    testlist.Add(jobj.Path.ToString().Split('.').Last());
                }
                testlist.RemoveAt(0);

                foreach (var search in searchList)
                {
                    var dates = testlist.FindAll(delegate (string s) { return s.Contains(search.Value); });
                    var month = search.Value.Substring(5, 2);
                    var year = search.Value.Substring(2, 2);
                    var day = int.Parse(dates[0].Substring(8));
                    if (day <= 26 && dates[0] != testlist.First())
                    {
                        ADX.Add(search.Key, testlist[testlist.IndexOf(dates[0]) - 1]);
                    }
                    else
                    {
                        ADX.Add(search.Key, dates[0]);
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetADX", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public void GetBBANDS()
        {
            var request = new RestRequest($"query?function=BBANDS&symbol=AAPL&interval=weekly&time_period=5&series_type=close&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            try //Should only catch API errors, otherwise will return as normal
            {
                var result = json["Technical Analysis: BBANDS"].Children().ToList();
                var testlist = new List<string>();
                foreach (var jobj in result)
                {
                    testlist.Add(jobj.Path.ToString().Split('.').Last());
                }
                testlist.RemoveAt(0);

                foreach (var search in searchList)
                {
                    var dates = testlist.FindAll(delegate (string s) { return s.Contains(search.Value); });
                    var month = search.Value.Substring(5, 2);
                    var year = search.Value.Substring(2, 2);
                    var day = int.Parse(dates[0].Substring(8));
                    if (day <= 26 && dates[0] != testlist.First())
                    {
                        BBANDS.Add(search.Key, testlist[testlist.IndexOf(dates[0]) - 1]);
                    }
                    else
                    {
                        BBANDS.Add(search.Key, dates[0]);
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetBBANDS", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public void GetBOP()
        {
            var request = new RestRequest($"query?function=BOP&symbol=AAPL&interval=monthly&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            try //Should only catch API errors, otherwise will return as normal
            {
                var result = json["Technical Analysis: BOP"].Children().ToList();
                var testlist = new List<string>();
                foreach (var jobj in result)
                {
                    testlist.Add(jobj.Path.ToString().Split('.').Last());
                }
                testlist.RemoveAt(0);

                foreach (var search in searchList)
                {
                    var dates = testlist.FindAll(delegate (string s) { return s.Contains(search.Value); });
                    BOP.Add(search.Key, dates[0]);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetBOP", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        public void GetMACD()
        {
            var request = new RestRequest($"query?function=MACD&symbol=AAPL&interval=daily&series_type=open&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            try //Should only catch API errors, otherwise will return as normal
            {
                var result = json["Technical Analysis: MACD"].Children().ToList();
                var testlist = new List<string>();
                foreach (var jobj in result)
                {
                    testlist.Add(jobj.Path.ToString().Split('.').Last());
                }
                testlist.RemoveAt(0);

                foreach (var search in searchList)
                {
                    var dates = testlist.FindAll(delegate (string s) { return s.Contains(search.Value); });
                    MACD.Add(search.Key, dates[0]);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetMACD", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        public void GetMOM()
        {
            var request = new RestRequest($"query?function=MOM&symbol=AAPL&interval=daily&time_period=10&series_type=close&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            try //Should only catch API errors, otherwise will return as normal
            {
                var result = json["Technical Analysis: MOM"].Children().ToList();
                var testlist = new List<string>();
                foreach (var jobj in result)
                {
                    testlist.Add(jobj.Path.ToString().Split('.').Last());
                }
                testlist.RemoveAt(0);

                foreach (var search in searchList)
                {
                    var dates = testlist.FindAll(delegate (string s) { return s.Contains(search.Value); });
                    MOM.Add(search.Key, new List<string>()
                        {
                            dates[0],
                            dates[1],
                            dates[2],
                            dates[3],
                            dates[4]
                        });
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetMOM", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public void GetRSI()
        {
            var request = new RestRequest($"query?function=RSI&symbol=AAPL&interval=weekly&time_period=10&series_type=close&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            try //Should only catch API errors, otherwise will return as normal
            {
                var result = json["Technical Analysis: RSI"].Children().ToList();
                var testlist = new List<string>();
                foreach (var jobj in result)
                {
                    testlist.Add(jobj.Path.ToString().Split('.').Last());
                }
                testlist.RemoveAt(0);

                foreach (var search in searchList)
                {
                    var dates = testlist.FindAll(delegate (string s) { return s.Contains(search.Value); });
                    var month = search.Value.Substring(5, 2);
                    var year = search.Value.Substring(2, 2);
                    var day = int.Parse(dates[0].Substring(8));
                    if (day <= 26 && dates[0] != testlist.First())
                    {
                        RSI.Add(search.Key, testlist[testlist.IndexOf(dates[0]) - 1]);
                    }
                    else
                    {
                        RSI.Add(search.Key, dates[0]);
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetRSI", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public void GetGain()
        {
            var request = new RestRequest($"query?function=TIME_SERIES_DAILY&symbol=AAPL&outputsize=full&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            try //Should only catch API errors, otherwise will return as normal
            {
                var result = json["Time Series (Daily)"].Children().ToList();
                var testlist = new List<string>();
                foreach (var jobj in result)
                {
                    testlist.Add(jobj.Path.ToString().Split('.').Last());
                }
                testlist.RemoveAt(0);

                foreach (var search in searchList)
                {
                    var dates = testlist.FindAll(delegate (string s) { return s.Contains(search.Value); });
                    Gain.Add(search.Key, new List<string> { dates.Last(), dates[0] });
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetGain", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        public void GetPrice()
        {
            var request = new RestRequest($"query?function=TIME_SERIES_DAILY&symbol=AAPL&outputsize=full&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            try //Should only catch API errors, otherwise will return as normal
            {
                var result = json["Time Series (Daily)"].Children().ToList();
                var testlist = new List<string>();
                foreach (var jobj in result)
                {
                    testlist.Add(jobj.Path.ToString().Split('.').Last());
                }
                testlist.RemoveAt(0);

                foreach (var search in searchList)
                {
                    var dates = testlist.FindAll(delegate (string s) { return s.Contains(search.Value); });
                    Price.Add(search.Key, dates[0]);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetPrice", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        private JObject GetJObject(RestRequest request)
        {
            System.Threading.Thread.Sleep(delay);
            JObject json = null;
            string info;
            var returned = client.Execute(request); //gathers response
            try
            {
                json = JObject.Parse(returned.Content); //converts to queryable object
                info = (string)json["Information"]; //Checks if API returned call timer exceeded and retries
            }
            catch
            {
                info = "Thank you for using Alpha Vantage! Please visit https://www.alphavantage.co/premium/ if you would like to have a higher API call volume.";
            }

            while (info == "Thank you for using Alpha Vantage! Please visit https://www.alphavantage.co/premium/ if you would like to have a higher API call volume.")
            {
                System.Threading.Thread.Sleep(delay);
                try
                {
                    returned = client.Execute(request);
                    json = JObject.Parse(returned.Content); //converts to queryable object
                    info = (string)json["Information"]; //Checks if API returned call timer exceeded and retries
                }
                catch
                {
                    info = "Thank you for using Alpha Vantage! Please visit https://www.alphavantage.co/premium/ if you would like to have a higher API call volume.";
                }
            }

            return json;
        }

        public dynamic GetDate(string fundamental, string month) //Returns the correct date for the month and fundamental passed in
        {
            switch (fundamental)
            {
                case ("ADX"):
                    return ADX[month];
                case ("BBANDS"):
                    return BBANDS[month];
                case ("BOP"):
                    return BOP[month];
                case ("MACD"):
                    return MACD[month];
                case ("MOM"):
                    return MOM[month];
                case ("RSI"):
                    return RSI[month];
                case ("Price"):
                    return Price[month];
                case ("Gain"):
                    return Gain[month];
                default:
                    return null;
            }
        }

        public void GetSearchMonths()
        {
            var stringList = GetMonths();

            foreach (var monthString in stringList)
            {
                var monthInt = int.Parse(Months[monthString.Substring(0, monthString.Length - 2)]) - 1;
                var year = int.Parse(monthString.Substring(monthString.Length - 2));

                if (monthInt == 0)
                {
                    monthInt = 12;
                    year--;
                }

                string month;

                if (monthInt < 10)
                {
                    month = $"0{monthInt}";
                }
                else
                {
                    month = monthInt.ToString();
                }

                searchList.Add(monthString, $"20{year}-{month}-");
            }
        }

        private List<string> GetMonths()
        {
            string[] monthArray = Directory.GetDirectories(@"..\..\..\Files\Earnings\");
            var monthList = new List<string>();
            foreach (string path in monthArray)
            {
                monthList.Add(path.Split('\\')[5]);
            }
            return monthList;
        }
    }
}
