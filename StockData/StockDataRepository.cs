using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockData
{
    public class StockDataRepository
    {
        public StockData Retrieve()
        {
            var stocks = new StockData()
            {
                Symbol = "AAPL",
                Price = 95,
                EarningsGrowth = .45,
                ADX = 35,
                BBANDS = .12354,
                BOP = -.454,
                MACD = -2.79,
                MOM = .0345,
                RSI = 40,
                PriceGain = 1.1,
            };

            return stocks;
        }
    }
}
