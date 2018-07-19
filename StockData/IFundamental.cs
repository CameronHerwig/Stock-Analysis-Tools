using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockData
{
    public interface IFundamental
    {
        double ADX { get; set; }
        double BBANDS { get; set; }
        double BOP { get; set; }
        double MACD { get; set; }
        double MOM { get; set; }
        double RSI { get; set; }
        double Gain { get; set; }
    }
}
