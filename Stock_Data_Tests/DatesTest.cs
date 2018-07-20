using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stock_Data;

namespace Stock_Data_Tests
{
    [TestClass]
    public class DatesTest
    {
        [TestMethod]
        public void GetDateTest()
        {
            // Arrange
            var date = new Dates();
            var expected = "2017-09-01";
            // Act
            var actual = date.GetDate("RSI", "September17");

            // Assert
            Assert.AreEqual(expected, (string)actual);
        }

        [TestMethod]
        public void GetDateTestNoFund()
        {
            // Arrange
            var date = new Dates();
            var expected = "2017-09-01";
            // Act
            var actual = date.GetDate("","September17");

            // Assert
            Assert.AreNotEqual(expected, (string)actual);
        }
    }
}
