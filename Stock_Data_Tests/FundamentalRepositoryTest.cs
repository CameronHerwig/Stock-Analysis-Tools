using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock_Data;

namespace Stock_Data_Tests
{
    [TestClass]
    public class FundamentalRepositoryTest
    {
        [TestMethod]
        public void SaveFundamentalsTest()
        {
            //Arrange
            var fundamentals = new FundamentalRepository();
            var stockData = new List<IFundamental>()
            {
                new Fundamental { Symbol = "Test1", ADX = 1, BBANDS= 2, BOP = 3, MACD = 4, MOM = 5, RSI = 6, Gain = 7 },
                new Fundamental { Symbol = "Test2", ADX = 2, BBANDS= 3, BOP = 4, MACD = 5, MOM = 6, RSI = 7, Gain = 8 },
                new Fundamental { Symbol = "Test3", ADX = 3, BBANDS= 4, BOP = 5, MACD = 6, MOM = 7, RSI = 8, Gain = 9 }
            };

            string month = "TestMonth";

            //Act
            fundamentals.SaveFundamentals(stockData, month);

            //Assert 
            //NO ASSERTATION
        }

        [TestMethod]
        public void SubsetsTest()
        {
            var fundamentals = new FundamentalChooser();
            var stock = new StockDataRepository();
            var set = new List<string>
            {
                "ADX","BBANDS","BOP","MACD","MOM","RSI"
            };

            var result = fundamentals.SubSetsOf(set);

            var stockData = stock.RetrieveHTML("October17");
            stockData = stock.RetrieveFundamentals("October17");

            var stockTest = new StockData
            {
                Symbol = "TEST",
                Price = 10,
                EarningsGrowth = 1,
                ADX = 30,
                BBANDS = .5,
                BOP = -.03,
                MACD = -2,
                MOM = -.03,
                RSI = 40,
                Gain = 1.07,
                ForwardPE = 4
            };
            foreach(var symbol in stockData)
            {
                foreach (var list in result)
                {
                    fundamentals.BuildComparison((StockData)symbol, list.Distinct().ToList());
                }
            }            
        }
    }

}


