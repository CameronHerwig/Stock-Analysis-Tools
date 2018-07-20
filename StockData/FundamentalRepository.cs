﻿using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Stock_Data
{
    public class FundamentalRepository
    {
        Dates dateClass = new Dates();
        private readonly int delay = 15000;
        RestClient client = new RestClient("https://www.alphavantage.co/");
        private readonly string apiKey = "CP85DS06PVNPIF7X";

        public double GetADX(string symbol, string month)
        {          
            var request = new RestRequest($"query?function=ADX&symbol={symbol}&interval=weekly&time_period=10&apikey={apiKey}");

            JObject json = GetJObject(request);

            var date = dateClass.GetDate("ADX", month);

            var search = json["Technical Analysis: ADX"][date]["ADX"];
            if(search == null)
            {
                return 0;
            }

            var ADX = (double)search;

            return ADX;
        }

        public double GetBBANDS(string symbol, string month, double price)
        {
            var request = new RestRequest($"query?function=BBANDS&symbol={symbol}&interval=weekly&time_period=5&series_type=close&apikey={apiKey}");

            JObject json = GetJObject(request);

            var date = dateClass.GetDate("BBANDS", month);

            var search = json["Technical Analysis: BBANDS"][date]["Real Upper Band"];
            if (search == null)
            {
                return 0;
            }

            var BBANDS = ((double)search - (double)json["Technical Analysis: BBANDS"][date]["Real Lower Band"]) / price;

            return BBANDS;
        }

        public double GetBOP(string symbol, string month)
        {
            var request = new RestRequest($"query?function=BOP&symbol={symbol}&interval=monthly&apikey={apiKey}");

            JObject json = GetJObject(request);

            var date = dateClass.GetDate("BOP", month);

            var search = json["Technical Analysis: BOP"][date]["BOP"];
            if (search == null)
            {
                return 0;
            }

            var BOP = (double)search;

            return BOP;
        }

        public double GetMACD(string symbol, string month)
        {
            var request = new RestRequest($"query?function=MACD&symbol={symbol}&interval=daily&series_type=open&apikey={apiKey}");

            JObject json = GetJObject(request);

            var date = dateClass.GetDate("MACD", month);

            var search = json["Technical Analysis: MACD"][date]["MACD"];
            if (search == null)
            {
                return 0;
            }

            var MACD = (double)search;

            return MACD;
        }

        public double GetMOM(string symbol, string month, double price)
        {
            var request = new RestRequest($"query?function=MOM&symbol={symbol}&interval=daily&time_period=10&series_type=close&apikey={apiKey}");

            JObject json = GetJObject(request);

            var date = dateClass.GetDate("MOM", month);

            var search = json["Technical Analysis: MOM"][date[4]]["MOM"];
            if (search == null)
            {
                return 0;
            }

            var mom1 = (double)json["Technical Analysis: MOM"][date[0]]["MOM"];
            var mom2 = (double)json["Technical Analysis: MOM"][date[1]]["MOM"];
            var mom3 = (double)json["Technical Analysis: MOM"][date[2]]["MOM"];
            var mom4 = (double)json["Technical Analysis: MOM"][date[3]]["MOM"];
            var mom5 = (double)search;

            var MOM = ((mom1 + mom2 + mom3 + mom4 + mom5)/(5*price));

            return MOM;
        }

        public double GetRSI(string symbol, string month)
        {
            var request = new RestRequest($"query?function=RSI&symbol={symbol}&interval=weekly&time_period=10&series_type=close&apikey={apiKey}");

            JObject json = GetJObject(request);

            var date = dateClass.GetDate("RSI", month);

            var search = json["Technical Analysis: RSI"][date]["RSI"];
            if (search == null)
            {
                return 0;
            }

            var RSI = (double)search;

            return RSI;
        }

        public double GetGain(string symbol, string month)
        {
            var request = new RestRequest($"query?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=full&apikey={apiKey}");

            JObject json = GetJObject(request);

            var date = dateClass.GetDate("Gain", month);

            var search = json["Time Series (Daily)"][date[0]]["2. high"];
            if (search == null)
            {
                return 0;
            }

            var buy = (double)search;
            var sell = (double)json["Time Series (Daily)"][date[1]]["2. high"];

            return ((sell-buy)/buy);
        }

        public double GetPrice(string symbol, string month)
        {
            var request = new RestRequest($"query?function=TIME_SERIES_DAILY&symbol={symbol}&outputsize=full&apikey={apiKey}");

            JObject json = GetJObject(request);

            var date = dateClass.GetDate("Price", month);
            double price;

            try
            {
                var search = json["Time Series (Daily)"][date]["4. close"];
                if (search == null)
                {
                    return 0;
                }

                price = (double)search;
            }
            catch(Exception Ex)
            {
                MessageBox.Show(Ex.Message, "GetPrice Error", MessageBoxButton.OK, MessageBoxImage.Error);
                price = 0;
            }

            return price;
        }

        private JObject GetJObject(RestRequest request)
        {
            System.Threading.Thread.Sleep(delay);
            var returned = client.Execute(request);

            JObject json = JObject.Parse(returned.Content);

            var info = (string)json["Information"];
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
            var headers = "Symbol, ADX, BBANDS, BOP, MACD, MOM, RSI, Gain";
            var path = $@"C:\Users\cherw\Desktop\Github\Stock Analysis Tools\Files\Fundamentals\{month}.csv";
            using (var file = File.CreateText(path))
            {
                file.WriteLine(headers);
                foreach (var symbol in fundamentalData)
                {
                    file.WriteLine(string.Join(",", symbol.Symbol, symbol.ADX, symbol.BBANDS, symbol.BOP, symbol.MACD, symbol.MOM, symbol.RSI, symbol.Gain));
                }
            }
        }
    }
}
