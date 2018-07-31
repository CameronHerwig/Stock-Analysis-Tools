using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;

namespace Stock_Data
{
    public class FundamentalRepository
    {
        Dates dateClass = new Dates();
        private readonly int delay = int.Parse(ConfigurationManager.AppSettings["APIDelay"]); //Delay as mandated by API
        RestClient client = new RestClient("https://www.alphavantage.co/"); //Preps client for calls
        private readonly string apiKey = ConfigurationManager.AppSettings["APIKey"]; //Change as needed

        public double GetADX(string symbol, string month, bool showErrors)
        {          
            var request = new RestRequest($"query?function=ADX&symbol={symbol}&interval=weekly&time_period=10&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            var date = dateClass.GetDate("ADX", month); //Gathers date for query

            double ADX = 0;
            string search;

            try //Should only catch API errors, otherwise will return as normal
            {
                search = json["Technical Analysis: ADX"][date]["ADX"];
                if (search == null)
                {
                    return 0;
                }

                ADX = double.Parse(search);
            }
            catch (Exception Ex)
            {
                if (showErrors)
                {
                    MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetADX", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                ADX = 0;
            }

            return ADX;
        }

        public double GetBBANDS(string symbol, string month, double price, bool showErrors)
        {
            var request = new RestRequest($"query?function=BBANDS&symbol={symbol}&interval=weekly&time_period=5&series_type=close&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            var date = dateClass.GetDate("BBANDS", month); //Gathers date for query

            double BBANDS = 0;
            string search;

            try //Should only catch API errors, otherwise will return as normal
            {
                search = json["Technical Analysis: BBANDS"][date]["Real Upper Band"];
                if (search == null)
                {
                    return 0;
                }

                BBANDS = (double.Parse(search) - (double)json["Technical Analysis: BBANDS"][date]["Real Lower Band"]) / price;
            }
            catch (Exception Ex)
            {
                if (showErrors)
                {
                    MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetBBANDS", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                BBANDS = 0;
            }

            return BBANDS;
        }

        public double GetBOP(string symbol, string month, bool showErrors)
        { 
            var request = new RestRequest($"query?function=BOP&symbol={symbol}&interval=monthly&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            var date = dateClass.GetDate("BOP", month); //Gathers date for query

            double BOP = 0;
            string search;

            try //Should only catch API errors, otherwise will return as normal
            {
                search = json["Technical Analysis: BOP"][date]["BOP"];
                if (search == null)
                {
                    return 0;
                }

                BOP = double.Parse(search);
        }
            catch (Exception Ex)
            {
                if (showErrors)
                {
                    MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetBOP", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                BOP = 0;
            }


            return BOP;
        }

        public double GetMACD(string symbol, string month, bool showErrors)
        {
            var request = new RestRequest($"query?function=MACD&symbol={symbol}&interval=daily&series_type=open&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            var date = dateClass.GetDate("MACD", month); //Gathers date for query

            double MACD = 0;

            try //Should only catch API errors, otherwise will return as normal
            {
                var search = json["Technical Analysis: MACD"][date]["MACD"];
                if (search == null)
                {
                    return 0;
                }

                MACD = (double)search;
            }
            catch (Exception Ex)
            {
                if (showErrors)
                {
                    MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetMACD", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                MACD = 0;
            }

            return MACD;
        }

        public double GetMOM(string symbol, string month, double price, bool showErrors)
        {
            var request = new RestRequest($"query?function=MOM&symbol={symbol}&interval=daily&time_period=10&series_type=close&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            var date = dateClass.GetDate("MOM", month); //Gathers date for query

            double MOM = 0;
            string search;
            double mom1;
            double mom2;
            double mom3;
            double mom4;
            double mom5;

            try //Should only catch API errors, otherwise will return as normal
            {
                search = json["Technical Analysis: MOM"][date[4]]["MOM"];
                if (search == null)
                {
                    return 0;
                }

                mom1 = (double)json["Technical Analysis: MOM"][date[0]]["MOM"];
                mom2 = (double)json["Technical Analysis: MOM"][date[1]]["MOM"];
                mom3 = (double)json["Technical Analysis: MOM"][date[2]]["MOM"];
                mom4 = (double)json["Technical Analysis: MOM"][date[3]]["MOM"];
                mom5 = double.Parse(search);

                MOM = ((mom1 + mom2 + mom3 + mom4 + mom5) / (5 * price));
                }
            catch (Exception Ex)
            {
                if (showErrors)
                {
                    MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetMOM", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                MOM = 0;
            }

            return MOM;
        }

        public double GetRSI(string symbol, string month, bool showErrors)
        {
            var request = new RestRequest($"query?function=RSI&symbol={symbol}&interval=weekly&time_period=10&series_type=close&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            var date = dateClass.GetDate("RSI", month); //Gathers date for query

            double RSI;
            string search;

            try //Should only catch API errors, otherwise will return as normal
            {
                search = json["Technical Analysis: RSI"][date]["RSI"];
                if (search == null)
                {
                    return 0;
                }
                RSI = double.Parse(search);
            }
            catch (Exception Ex)
            {
                if (showErrors)
                {
                    MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetRSI", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                RSI = 0;
            }

            return RSI;
        }

        public double GetGain(string symbol, string month, bool showErrors)
        {
            var request = new RestRequest($"query?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=full&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            var date = dateClass.GetDate("Gain", month); //Gathers date for query

            double gain = 0;
            string search;
            double buy;
            double sell;

            try //Should only catch API errors, otherwise will return as normal    
            {
                search = json["Time Series (Daily)"][date[0]]["2. high"];
                if (search == null)
                {
                    return 0;
                }

                buy = double.Parse(search);
                sell = (double)json["Time Series (Daily)"][date[1]]["2. high"];
                gain = 1 + ((sell - buy) / buy);
            }
            catch (Exception Ex)
            {
                if (showErrors)
                {
                    MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetGain", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                gain = 0;
            }

            return gain;
        }

        public double GetPrice(string symbol, string month, bool showErrors)
        {
            var request = new RestRequest($"query?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=full&apikey={apiKey}"); //Builds individual request

            JObject json = GetJObject(request); //Gets JSON

            var date = dateClass.GetDate("Price", month); //Gathers date for query
            double price;
            string search;

            try //Should only catch API errors, otherwise will return as normal
            {
                search = json["Time Series (Daily)"][date]["4. close"];
                if (search == null)
                {
                    return 0;
                }

                price = double.Parse(search);
            }
            catch (Exception Ex)
            {
                if(showErrors)
                {
                    MessageBox.Show(Ex.Message, "Stock_Data:FundamentalRepository:GetPrice", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                price = 0;
            }

            return price;
        }

        private JObject GetJObject(RestRequest request)
        {
            System.Threading.Thread.Sleep(delay);
            var returned = client.Execute(request); //gathers response

            JObject json = JObject.Parse(returned.Content); //converts to queryable object

            var info = (string)json["Information"]; //Checks if API returned call timer exceeded and retries
            while (info == "Thank you for using Alpha Vantage! Please visit https://www.alphavantage.co/premium/ if you would like to have a higher API call volume.")
            {
                System.Threading.Thread.Sleep(delay);
                returned = client.Execute(request);
                json = JObject.Parse(returned.Content);
                info = (string)json["Information"];
            }

            return json;
        }

        public void SaveFundamentals(List<IFundamental> fundamentalData, string month)
        {
            var headers = "Symbol, ADX, BBANDS, BOP, MACD, MOM, RSI, Gain"; //sets header string
            var path = $@"..\..\..\Files\Fundamentals\{month}.csv";
            using (var file = File.CreateText(path))
            {
                file.WriteLine(headers); //writes headers
                foreach (var symbol in fundamentalData) //writes whole list
                {
                    file.WriteLine(string.Join(",", symbol.Symbol, symbol.ADX, symbol.BBANDS, symbol.BOP, symbol.MACD, symbol.MOM, symbol.RSI, symbol.Gain));
                }
            }
        }

        public void SaveFutureFundamentals(List<IFutureData> fundamentalData, string month)
        {
            var headers = "Symbol, Price, ADX, BBANDS, BOP, MACD, MOM, RSI"; //sets header string
            var path = $@"..\..\..\Files\FutureFundamentals\{month}.csv";
            using (var file = File.CreateText(path))
            {
                file.WriteLine(headers); //writes headers
                foreach (var symbol in fundamentalData) //writes whole list
                {
                    file.WriteLine(string.Join(",", symbol.Symbol, symbol.Price, symbol.ADX, symbol.BBANDS, symbol.BOP, symbol.MACD, symbol.MOM, symbol.RSI));
                }
            }
        }
    }
}
