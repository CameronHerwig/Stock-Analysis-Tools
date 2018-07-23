using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock_Data;

namespace Stock_Data_Tests
{
    [TestClass]
    public class StockDataRepositoryTest
    {
        [TestMethod]
        public void GetMonthsTest()
        {
            //Arrange
            var stockData = new StockDataRepository();

            //Act
            var months = stockData.GetMonths();          
            bool gathered = months.Contains("October17");

            //Assert
            Assert.AreEqual(gathered, true);

        }

        [TestMethod]
        public void RetrieveHTMLTest()
        {
            //Arrange
            var stockData = new StockDataRepository();

            //Act
            var retrieved = stockData.RetrieveHTML("September17");

            //Assert
            Assert.AreEqual("ABMD", retrieved[0].Symbol);

        }

        [TestMethod]
        public void RetrieveFundamentalsTest()
        {
            //Arrange
            var stockData = new StockDataRepository();

            //Act
            stockData.RetrieveHTML("September17");
            var retrieved = stockData.RetrieveFundamentals("September17");
            var notSet = retrieved.FindAll(symbol => symbol.ADX == 0);

            //Assert
            Assert.AreEqual(notSet.Count, 0);
        }
    }
}
