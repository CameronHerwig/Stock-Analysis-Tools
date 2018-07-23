using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Stock_Data
{
    public class StockDataRepository
    {
        private List<IStockData> _stockData;

        public List<IStockData> RetrieveHTML(string month)
        {

            var path = $"C:\\Users\\cherw\\Desktop\\Github\\Stock Analysis Tools\\Files\\Earnings\\{month}\\"; //uses interpolation to gather month specific data
            string[] foundArray = Directory.GetFiles(path, "*.html"); //grab html files

            //string[] excludeSM = Directory.GetFiles(@"C:\Users\cherw\Desktop\Github\Stock Analysis Tools\Files\Earnings", "*sm.21.html", SearchOption.AllDirectories); //sm.21 is included in DLs and is a dead file
            //string[] excludeSA = Directory.GetFiles(@"C:\Users\cherw\Desktop\Github\Stock Analysis Tools\Files\Earnings", "*smartads.html", SearchOption.AllDirectories); //smartads is included in DLs and is a dead file
            //var excludeArray = excludeSM.Concat(excludeSA)
            //             .Where(x => !string.IsNullOrEmpty(x)) //probably
            //             .ToArray();
            //string[] fileArray = foundArray.Except(excludeArray).ToArray(); //filter

            string[] fileArray = foundArray; //placeholder if excludes needed later

            var stockList = new List<IStockData>();

            if (fileArray != null)
            {
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
                                Price = Math.Round(price,4), 
                                EarningsGrowth = Math.Round(growth,4),
                                ForwardPE = Math.Round(price / (double.Parse(symbol[3]) * 4), 4)
                            });
                        }
                    }
                }

                stockList.Sort((p, q) => p.Symbol.CompareTo(q.Symbol)); //defaults to alphabetical sort

                _stockData = stockList; //sets private variable               
            }

            return _stockData;
        }

        public List<IStockData> GatherFundamentals(string month)
        {
            var fundamentals = new FundamentalRepository();
            var fundList = new List<IFundamental>();
            string[] fileArray = Directory.GetFiles(@"C:\Users\cherw\Desktop\Github\Stock Analysis Tools\Files\Fundamentals", $"{month}.csv", SearchOption.AllDirectories); //gets months fundamentals

            if (fileArray.Length != 0)
            {
                return RetrieveFundamentals(month);
            }
            else
            {
                var stockData = _stockData;

                foreach(var stock in stockData)
                {                    
                    var price = fundamentals.GetPrice(stock.Symbol, month);
                    stock.ADX = fundamentals.GetADX(stock.Symbol, month);
                    stock.BBANDS = fundamentals.GetBBANDS(stock.Symbol, month, price);
                    stock.BOP = fundamentals.GetBOP(stock.Symbol, month);
                    stock.MACD = fundamentals.GetMACD(stock.Symbol, month);
                    stock.MOM = fundamentals.GetMOM(stock.Symbol, month, price);
                    stock.RSI = fundamentals.GetRSI(stock.Symbol, month);
                    stock.Gain = fundamentals.GetGain(stock.Symbol, month);

                    fundList.Add(new Fundamental()
                    {
                        Symbol = stock.Symbol,
                        ADX = stock.ADX,
                        BBANDS = stock.BBANDS,
                        BOP = stock.BOP ,
                        MACD = stock.MACD,
                        MOM = stock.MOM,
                        RSI = stock.RSI,
                        Gain = stock.Gain
                    });
                }

                _stockData.RemoveAll(symbol => symbol.ADX == 0);
                fundList.RemoveAll(symbol => symbol.ADX == 0);

                fundamentals.SaveFundamentals(fundList,month);

                return _stockData;
            }
        }

        public List<IComparisonResult> GetComparisons(string month)
        {

            string[] fileArray = Directory.GetFiles(@"C:\Users\cherw\Desktop\Github\Stock Analysis Tools\Files\Results", $"{month}.csv", SearchOption.AllDirectories); //gets months fundamentals
            var fundamentals = new FundamentalChooser();

            if (fileArray.Length != 0)
            {
                return fundamentals.RetrieveComparisons(month);
            }
            else
            {
                var set = new List<string>
                {
                "ADX","BBANDS","BOP","MACD","MOM","RSI"
                };
                var subSets = fundamentals.SubSetsOf(set).Distinct().ToList();

                var stockData = _stockData;

                foreach (var symbol in stockData)
                {
                    foreach (var fundSet in subSets)
                    {
                        fundamentals.BuildComparison((StockData)symbol, fundSet.ToList());
                    }
                }

                var comparisonResults = fundamentals.successRate;
                comparisonResults["Default"] = comparisonResults[""];
                comparisonResults.Remove("");

                var totalResult = new List<IComparisonResult>();

                foreach (var result in comparisonResults)
                {
                    double success = result.Value["Success"];
                    double total = result.Value["Total"];
                    double successPercent = success / total;
                    totalResult.Add(new ComparisonResult
                    {
                        TestName = result.Key,
                        SuccessPercent = Math.Round(successPercent, 4),
                        Total = total,
                        Success = success
                    });
                }

                fundamentals.SaveComparisons(totalResult, month);

                return totalResult;
            }            
        }

        public List<string> GetMonths()
        {
            string[] monthArray = Directory.GetDirectories(@"C:\Users\cherw\Desktop\Github\Stock Analysis Tools\Files\Earnings\");
            var monthList = new List<string>();
            foreach(string path in monthArray)
            {
                monthList.Add(path.Split('\\')[8]);
            }
            return monthList;
        }

        public List<IStockData> RetrieveFundamentals(string month)
        {
            string[] fileArray = Directory.GetFiles(@"C:\Users\cherw\Desktop\Github\Stock Analysis Tools\Files\Fundamentals", $"{month}.csv", SearchOption.AllDirectories); //gets months fundamentals

            if(fileArray != null)
            {
                var fundamentalList = new Dictionary<string, List<string>>(); //main dictionary

                foreach (string file in fileArray)
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

                foreach (var symbol in _stockData.ToList())
                {
                    if (fundamentalList.ContainsKey(symbol.Symbol)) //sets found symbol's data into object fields
                    {
                        symbol.ADX = Math.Round(double.Parse(fundamentalList[symbol.Symbol][0]),4);
                        symbol.BBANDS = Math.Round(double.Parse(fundamentalList[symbol.Symbol][1]), 4);
                        symbol.BOP = Math.Round(double.Parse(fundamentalList[symbol.Symbol][2]), 4);
                        symbol.MACD = Math.Round(double.Parse(fundamentalList[symbol.Symbol][3]), 4);
                        symbol.MOM = Math.Round(double.Parse(fundamentalList[symbol.Symbol][4]), 4);
                        symbol.RSI = Math.Round(double.Parse(fundamentalList[symbol.Symbol][5]), 4);
                        symbol.Gain = Math.Round(double.Parse(fundamentalList[symbol.Symbol][6]), 4);
                    }
                }

                _stockData.RemoveAll(symbol => symbol.ADX == 0); //removes symbols missing fundamentals
            }

            return _stockData;
        }
    }
}
