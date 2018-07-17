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
        private List<IStockData> _stockData;

        public List<IStockData> RetrieveHTML(string month)
        {

            var path = $"C:\\Users\\cherw\\Desktop\\Github\\Stock Analysis Tools\\Files\\Earnings\\{month}\\"; //uses interpolation to gather month specific data
            string[] foundArray = Directory.GetFiles(path, "*.html", SearchOption.AllDirectories); //grab html files
            string[] excludeArray = Directory.GetFiles(@"C:\Users\cherw\Desktop\Github\Stock Analysis Tools\Files\Earnings", "*sm.21.html", SearchOption.AllDirectories); //sm.21 is included in DLs and is a dead file
            string[] fileArray = foundArray.Except(excludeArray).ToArray(); //filter


            var tables = new List<List<List<string>>>(); //List of symbols and data in each file, and for each file
            HtmlDocument doc = new HtmlDocument();

            foreach (string filePath in fileArray) 
            {

                var currentFile = filePath;
                doc.Load(filePath);

                tables.Add(doc.DocumentNode?.SelectSingleNode("//tbody") //makes a list of cells for each row in the file (List<List<string>>)
                            .Descendants("tr")
                            .Skip(1)
                            .Where(tr => tr.Elements("td").Count() > 1)
                            .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                            .ToList());
            }

            var stockList = new List<IStockData>();

            foreach (var file in tables)
            {
                foreach (var symbol in file)
                {
                    var growth = double.Parse(symbol[3]) / double.Parse(symbol[5]); //gets current and last years earnings
                    var price = double.Parse(symbol[2]);
                    if (growth > 1.35 && price > 5) //35% growth and price > $5
                    {
                        stockList.Add(new StockData  //adds current info
                        {
                            Symbol = symbol[0].Split('/')[1],
                            Price = price,
                            EarningsGrowth = growth,
                            ForwardPE = price/(double.Parse(symbol[3])*4)
                        });
                    }
                }
            }

            stockList.Sort((p, q) => p.Symbol.CompareTo(q.Symbol)); //defaults to alphabetical sort

            _stockData = stockList; //sets private variable

            return stockList;
            
        }

        public List<IStockData> RetrieveFundamentals(string month)
        {
            string[] fileArray = Directory.GetFiles(@"C:\Users\cherw\Desktop\Github\Stock Analysis Tools\Files\Fundamentals", $"{month}.csv", SearchOption.AllDirectories); //gets months fundamentals

            var fundamentalList = new Dictionary<string, List<string>>(); //main dictionary

            foreach(string file in fileArray)
            {
                var fundamentals = File.ReadLines(file).Select(line => line.Split(',')).ToDictionary(line => line[0], line => new List<string> //grabs csv lines and makes dictionary indexed at symbol
                {
                    line[1],
                    line[2],
                    line[3],
                    line[4],
                    line[5],
                    line[6],
                    line[7],
                });
                fundamentals.Remove("Symbol"); //removes header line
                fundamentalList = fundamentalList.Concat(fundamentals).ToDictionary(x => x.Key, x => x.Value); //combines main and current dictionary
            }
            
            foreach(var symbol in _stockData.ToList())
            {
                if(fundamentalList.ContainsKey(symbol.Symbol)) //sets found symbol's data into object fields
                {
                    symbol.ADX = double.Parse(fundamentalList[symbol.Symbol][0]);
                    symbol.BBANDS = double.Parse(fundamentalList[symbol.Symbol][1]);
                    symbol.BOP = double.Parse(fundamentalList[symbol.Symbol][2]);
                    symbol.MACD = double.Parse(fundamentalList[symbol.Symbol][3]);
                    symbol.MOM = double.Parse(fundamentalList[symbol.Symbol][4]);
                    symbol.RSI = double.Parse(fundamentalList[symbol.Symbol][5]);
                    symbol.Gain = double.Parse(fundamentalList[symbol.Symbol][6]);
                }
            }

            _stockData.RemoveAll(symbol => symbol.ADX == 0); //removes symbols missing fundamentals

            return _stockData;
        }
    }
}
