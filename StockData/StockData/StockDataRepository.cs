using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;

namespace Stock_Data
{
    public class StockDataRepository
    {
        private List<IStockData> _stockData;
        readonly double minimumGrowth = double.Parse(ConfigurationManager.AppSettings["MinimumGrowth"]);
        readonly int minimumPrice = int.Parse(ConfigurationManager.AppSettings["MinimumPrice"]);
        readonly string testFolder = ConfigurationManager.AppSettings["MinimumGain"];
        public static int stockCount = 0;
        public static int fundCount = 0;
        private List<IFutureData> _fundList;

        public List<IStockData> RetrieveHTML(string month)
        {

            var path = $"..\\..\\..\\Files\\Earnings\\{month}\\"; //uses interpolation to gather month specific data
            string[] foundArray = Directory.GetFiles(path, "*.html"); //grab html files

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
                        if (growth > minimumGrowth && price > minimumPrice) //35% growth and price > $5
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

                _stockData = stockList.GroupBy(x => x.Symbol).Select(x => x.First()).ToList(); //sets private variable               
            }

            return _stockData;
        }

        public List<IStockData> GatherFundamentals(string month, bool showErrors)
        {
            var fundamentals = new FundamentalRepository();
            var fundList = new List<IFundamental>();

            string[] fileArray = Directory.GetFiles(@"..\..\..\Files\Fundamentals", $"{month}.csv", SearchOption.AllDirectories); //gets months fundamentals

            if (fileArray.Length != 0)
            {
                return RetrieveFundamentals(month);
            }
            else
            {
                var stockData = _stockData;
                stockCount = stockData.Count();

                foreach(var stock in stockData)
                {
                    fundCount = fundList.Count();
                    var price = Math.Round(fundamentals.GetPrice(stock.Symbol, month, showErrors), 4);
                    stock.ADX = Math.Round(fundamentals.GetADX(stock.Symbol, month, showErrors), 4);
                    stock.BBANDS = Math.Round(fundamentals.GetBBANDS(stock.Symbol, month, price, showErrors), 4);
                    stock.BOP = Math.Round(fundamentals.GetBOP(stock.Symbol, month, showErrors), 4);
                    stock.MACD = Math.Round(fundamentals.GetMACD(stock.Symbol, month, showErrors), 4);
                    stock.MOM = Math.Round(fundamentals.GetMOM(stock.Symbol, month, price, showErrors), 4);
                    stock.RSI = Math.Round(fundamentals.GetRSI(stock.Symbol, month, showErrors), 4);
                    stock.Gain = Math.Round(fundamentals.GetGain(stock.Symbol, month, showErrors), 4);

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

                ClearEmptyStockData(ref _stockData);
                ClearEmptyFundData(ref fundList);

                fundamentals.SaveFundamentals(fundList,month);

                return _stockData;
            }
        }

        public List<IFutureData> RetrieveFutureHTML(string month)
        {
            _fundList = new List<IFutureData>();

            var stockData = RetrieveHTML(month);

            foreach(var stock in stockData)
            {
                _fundList.Add(new FutureData()
                {
                    Symbol = stock.Symbol,
                    Price = stock.Price,
                    EarningsGrowth = stock.EarningsGrowth,
                    ForwardPE = stock.ForwardPE
                });
            }

            return _fundList;
        }
        public List<IFutureData> GatherFutureFundamentals(string month, bool showErrors)
        {
            var fundamentals = new FundamentalRepository();
            string[] fileArray = Directory.GetFiles(@"..\..\..\Files\FutureFundamentals\", $"{month}.csv", SearchOption.AllDirectories); //gets months fundamentals

            if (fileArray.Length != 0)
            {
                return RetrieveFutureFundamentals(month);
            }
            else
            {
                stockCount = _fundList.Count();
                fundCount = 0;

                foreach (var stock in _fundList)
                {                  
                    stock.Price = fundamentals.GetPrice(stock.Symbol, month, showErrors);
                    stock.ADX = Math.Round(fundamentals.GetADX(stock.Symbol, month, showErrors), 4);
                    stock.BBANDS = Math.Round(fundamentals.GetBBANDS(stock.Symbol, month, stock.Price, showErrors), 4);
                    stock.BOP = Math.Round(fundamentals.GetBOP(stock.Symbol, month, showErrors), 4);
                    stock.MACD = Math.Round(fundamentals.GetMACD(stock.Symbol, month, showErrors), 4);
                    stock.MOM = Math.Round(fundamentals.GetMOM(stock.Symbol, month, stock.Price, showErrors), 4);
                    stock.RSI = Math.Round(fundamentals.GetRSI(stock.Symbol, month, showErrors), 4);
                    fundCount++;
                }

                ClearEmptyStockData(ref _stockData);
                ClearEmptyFutureData(ref _fundList);

                fundamentals.SaveFutureFundamentals(_fundList, month);

                return _fundList;
            }
        }

        public List<IComparisonResult> GetComparisons(string month)
        {
            DirectoryInfo di = Directory.CreateDirectory($@"..\..\..\Files\Results\{testFolder}\");
            string[] fileArray = Directory.GetFiles($@"..\..\..\Files\Results\{testFolder}\", $"{month}.csv", SearchOption.TopDirectoryOnly); //gets months fundamentals
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
                    double avgGain = result.Value["TotalGain"] / total;
                    double successPercent = success / total;
                    totalResult.Add(new ComparisonResult
                    {
                        TestName = result.Key,
                        SuccessPercent = Math.Round(successPercent, 4),
                        Total = total,
                        Success = success,
                        AvgGain = avgGain
                    });
                }

                fundamentals.SaveComparisons(totalResult, month);

                return totalResult;
            }            
        }

        public List<string> GetMonths()
        {
            string[] monthArray = Directory.GetDirectories(@"..\..\..\Files\Earnings\");
            var monthList = new List<string>();
            foreach(string path in monthArray)
            {
                monthList.Add(path.Split('\\')[5]);
            }
            return monthList;
        }

        public List<IStockData> RetrieveFundamentals(string month)
        {
            string[] fileArray = Directory.GetFiles(@"..\..\..\Files\Fundamentals", $"{month}.csv", SearchOption.AllDirectories); //gets months fundamentals

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

                ClearEmptyStockData(ref _stockData);
            }

            return _stockData;
        }

        public List<IFutureData> RetrieveFutureFundamentals(string month)
        {
            string[] fileArray = Directory.GetFiles(@"..\..\..\Files\FutureFundamentals\", $"{month}.csv", SearchOption.AllDirectories); //gets months fundamentals
            _fundList = new List<IFutureData>();

            if (fileArray != null)
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

                var stockData = RetrieveHTML(month);

                foreach (var symbol in stockData.ToList())
                {
                    if (fundamentalList.ContainsKey(symbol.Symbol)) //sets found symbol's data into object fields
                    {
                        symbol.Price = symbol.ADX = double.Parse(fundamentalList[symbol.Symbol][0]);
                        symbol.ADX = Math.Round(double.Parse(fundamentalList[symbol.Symbol][1]), 4);
                        symbol.BBANDS = Math.Round(double.Parse(fundamentalList[symbol.Symbol][2]), 4);
                        symbol.BOP = Math.Round(double.Parse(fundamentalList[symbol.Symbol][3]), 4);
                        symbol.MACD = Math.Round(double.Parse(fundamentalList[symbol.Symbol][4]), 4);
                        symbol.MOM = Math.Round(double.Parse(fundamentalList[symbol.Symbol][5]), 4);
                        symbol.RSI = Math.Round(double.Parse(fundamentalList[symbol.Symbol][6]), 4);

                        _fundList.Add(new FutureData()
                        {
                            Symbol = symbol.Symbol,
                            Price = symbol.Price,
                            EarningsGrowth = symbol.EarningsGrowth,
                            ADX = symbol.ADX,
                            BBANDS = symbol.BBANDS,
                            BOP = symbol.BOP,
                            MACD = symbol.MACD,
                            MOM = symbol.MOM,
                            RSI = symbol.RSI,
                            ForwardPE = symbol.ForwardPE
                        });
                    }
                }

                ClearEmptyFutureData(ref _fundList); //removes symbols missing fundamentals
            }

            return _fundList;
        }

        public void ClearEmptyFundData(ref List<IFundamental> dataList )
        {
            dataList.RemoveAll(symbol => symbol.ADX == 0);
            dataList.RemoveAll(symbol => symbol.BBANDS == 0);
            dataList.RemoveAll(symbol => symbol.BOP == 0);
            dataList.RemoveAll(symbol => symbol.MACD == 0);
            dataList.RemoveAll(symbol => symbol.MOM == 0);
            dataList.RemoveAll(symbol => symbol.RSI == 0);
            dataList.RemoveAll(symbol => symbol.Gain == 0);
        }

        public void ClearEmptyStockData(ref List<IStockData> dataList)
        {
            dataList.RemoveAll(symbol => symbol.ADX == 0);
            dataList.RemoveAll(symbol => symbol.BBANDS == 0);
            dataList.RemoveAll(symbol => symbol.BOP == 0);
            dataList.RemoveAll(symbol => symbol.MACD == 0);
            dataList.RemoveAll(symbol => symbol.MOM == 0);
            dataList.RemoveAll(symbol => symbol.RSI == 0);
            dataList.RemoveAll(symbol => symbol.Gain == 0);
        }

        public void ClearEmptyFutureData(ref List<IFutureData> dataList)
        {
            dataList.RemoveAll(symbol => symbol.ADX == 0);
            dataList.RemoveAll(symbol => symbol.BBANDS == 0);
            dataList.RemoveAll(symbol => symbol.BOP == 0);
            dataList.RemoveAll(symbol => symbol.MACD == 0);
            dataList.RemoveAll(symbol => symbol.MOM == 0);
            dataList.RemoveAll(symbol => symbol.RSI == 0);
        }
    }

    class ItemEqualityComparer : IEqualityComparer<StockData>
    {
        public bool Equals(StockData x, StockData y)
        {
            // Two items are equal if their keys are equal.
            return x.Symbol == y.Symbol;
        }

        public int GetHashCode(StockData obj)
        {
            return obj.Symbol.GetHashCode();
        }
    }
}
