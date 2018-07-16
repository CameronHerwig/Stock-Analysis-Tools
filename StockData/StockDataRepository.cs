using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Data
{
    public class StockDataRepository
    {
        public StockData RetrieveTest()
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

        public List<StockData> RetrieveHTML
        {
            get
            {

                string[] foundArray = Directory.GetFiles(@"C:\Users\cherw\Desktop\Github\Stock Analysis Tools\Files", "*.html", SearchOption.AllDirectories);
                string[] excludeArray = Directory.GetFiles(@"C:\Users\cherw\Desktop\Github\Stock Analysis Tools\Files", "*sm.21.html", SearchOption.AllDirectories);
                string[] fileArray = foundArray.Except(excludeArray).ToArray();

                var tables = new List<List<List<string>>>();
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.OptionEmptyCollection = true;
                foreach (string filePath in fileArray)
                {

                    var currentFile = filePath;
                    doc.Load(filePath);

                    tables.Add(doc.DocumentNode?.SelectSingleNode("//tbody")
                                .Descendants("tr")
                                .Skip(1)
                                .Where(tr => tr.Elements("td").Count() > 1)
                                .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                                .ToList());
                }

                var stockList = new List<StockData>();

                foreach (var file in tables)
                {
                    foreach (var symbol in file)
                    {
                        var growth = double.Parse(symbol[3]) / double.Parse(symbol[5]);
                        var price = double.Parse(symbol[2]);
                        if (growth > 1.35 && price > 5)
                        {
                            stockList.Add(new StockData
                            {
                                Symbol = symbol[0].Split('/')[1],
                                Price = price,
                                EarningsGrowth = growth,
                                ForwardPE = price/(double.Parse(symbol[3])*4)
                            });
                        }
                    }
                }

                stockList.Sort((p, q) => p.Symbol.CompareTo(q.Symbol));

                return stockList;
            }
        }
    }
}
