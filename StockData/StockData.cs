using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockData
{
    public class StockData : IStockData
    {
        public string Symbol { get; set; }
        public double Price { get; set; }
        public double EarningsGrowth { get; set; }
        public double ADX { get; set; }
        public double BBANDS { get; set; }
        public double BOP { get; set; }
        public double MACD { get; set; }
        public double MOM { get; set; }
        public double RSI { get; set; }
        public double PriceGain { get; set; }
    }
}
