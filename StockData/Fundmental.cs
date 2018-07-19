using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockData
{
    public class Fundmental : IFundamental
    {
        public double ADX { get; set; }
        public double BBANDS { get; set; }
        public double BOP { get; set; }
        public double MACD { get; set; }
        public double MOM { get; set; }
        public double RSI { get; set; }
        public double Gain { get; set; }
    }
}
