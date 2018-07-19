using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockData
{
    public class FundamentalRepository
    {
        Dates dateClass = new Dates();
        public double GetADX(string symbol, string month)
        {
            var client = new RestClient("https://www.alphavantage.co/");
            var request = new RestRequest($"query?function=ADX&symbol={symbol}&interval=weekly&time_period=10&apikey=CP85DS06PVNPIF7X");           
            var returned = client.Execute(request);
           
            JObject json = JObject.Parse(returned.Content);
            var date = dateClass.GetDate("ADX", month);

            var search = json["Technical Analysis: ADX"][date]["AX"];
            if(search != null)
            {
                var ADX = (double)search;

                return ADX;
            }
            else
            {
                return 0;
            }
        }

        public double GetBBANDS(string symbol, string month, double price)
        {
            var client = new RestClient("https://www.alphavantage.co/");
            var request = new RestRequest($"query?function=BBANDS&symbol={symbol}&interval=weekly&time_period=5&series_type=close&apikey=CP85DS06PVNPIF7X");
            var returned = client.Execute(request);

            JObject json = JObject.Parse(returned.Content);
            var date = dateClass.GetDate("BBANDS", month);

            var search = json["Technical Analysis: BBANDS"][date]["Real Upper Band"];
            if (search != null)
            {
                var BBANDS = ((double)search - (double)json["Technical Analysis: BBANDS"][date]["Real Lower Band"]) / price;

                return BBANDS;
            }
            else
            {
                return 0;
            }
        }

        public double GetBOP(string symbol, string month)
        {
            var client = new RestClient("https://www.alphavantage.co/");
            var request = new RestRequest($"query?function=BOP&symbol={symbol}&interval=monthly&apikey=CP85DS06PVNPIF7X");
            var returned = client.Execute(request);

            JObject json = JObject.Parse(returned.Content);
            var date = dateClass.GetDate("BOP", month);

            var search = json["Technical Analysis: BOP"][date]["BOP"];
            if (search != null)
            {
                var BOP = (double)search;

                return BOP;
            }
            else
            {
                return 0;
            }
        }

        public double GetMACD(string symbol, string month)
        {
            var client = new RestClient("https://www.alphavantage.co/");
            var request = new RestRequest($"query?function=MACD&symbol={symbol}&interval=daily&series_type=open&apikey=CP85DS06PVNPIF7X");
            var returned = client.Execute(request);

            JObject json = JObject.Parse(returned.Content);
            var date = dateClass.GetDate("MACD", month);

            var search = json["Technical Analysis: MACD"][date]["MACD"];
            if (search != null)
            {
                var MACD = (double)search;

                return MACD;
            }
            else
            {
                return 0;
            }
        }

        public double GetMOM(string symbol, string month, double price)
        {
            var client = new RestClient("https://www.alphavantage.co/");
            var request = new RestRequest($"query?function=MOM&symbol={symbol}&interval=daily&time_period=10&series_type=close&apikey=CP85DS06PVNPIF7X");
            var returned = client.Execute(request);

            JObject json = JObject.Parse(returned.Content);
            var date = dateClass.GetDate("MOM", month);

            var search = json["Technical Analysis: MOM"][date[0]]["MOM"];
            if (search != null)
            {

                var mom1 = (double)search;
                var mom2 = (double)json["Technical Analysis: MOM"][date[1]]["MOM"];
                var mom3 = (double)json["Technical Analysis: MOM"][date[2]]["MOM"];
                var mom4 = (double)json["Technical Analysis: MOM"][date[3]]["MOM"];
                var mom5 = (double)json["Technical Analysis: MOM"][date[4]]["MOM"];

                var MOM = ((mom1 + mom2 + mom3 + mom4 + mom5)/(5*price));

                return MOM;
            }
            else
            {
                return 0;
            }
        }

        public double GetRSI(string symbol, string month)
        {
            var client = new RestClient("https://www.alphavantage.co/");
            var request = new RestRequest($"query?function=RSI&symbol={symbol}&interval=weekly&time_period=10&series_type=close&apikey=CP85DS06PVNPIF7X");
            var returned = client.Execute(request);

            JObject json = JObject.Parse(returned.Content);
            var date = dateClass.GetDate("RSI", month);

            var search = json["Technical Analysis: RSI"][date]["RSI"];
            if (search != null)
            {
                var RSI = (double)search;

                return RSI;
            }
            else
            {
                return 0;
            }
        }

        public double GetGain(string symbol, string month)
        {
            var client = new RestClient("https://www.alphavantage.co/");
            var request = new RestRequest($"query?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=full&apikey=CP85DS06PVNPIF7X");
            var returned = client.Execute(request);

            JObject json = JObject.Parse(returned.Content);
            var date = dateClass.GetDate("Gain", month);

            var search = json["Time Series (Daily)"][date[0]]["2. high"];
            if (search != null)
            {
                var buy = (double)search;
                var sell = (double)json["Time Series (Daily)"][date[1]]["2. high"];

                return ((sell-buy)/buy);
            }
            else
            {
                return 0;
            }
        }

        public double GetPrice(string symbol, string month)
        {
            var client = new RestClient("https://www.alphavantage.co/");
            var request = new RestRequest($"query?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=full&apikey=CP85DS06PVNPIF7X");
            var returned = client.Execute(request);

            JObject json = JObject.Parse(returned.Content);
            var date = dateClass.GetDate("Price", month);

            var search = json["Time Series (Daily)"][date]["4. close"];
            if (search != null)
            {
                var price = (double)search;

                return price;
            }
            else
            {
                return 0;
            }
        }
    }
}
