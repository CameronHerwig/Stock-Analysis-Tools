using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Data
{
    public class FundamentalChooser
    {
        private List<string> finished = new List<string>();

        public Dictionary<string, Dictionary<string, int>> successRate = new Dictionary<string, Dictionary<string, int>>();

        private readonly List<double> ADX = new List<double>
        {
            20, 25, 30, 35, 40, 45, 50
        };

        private readonly List<double> BBANDS = new List<double>
        {
            .07, .06, .05, .04, .03, .02
        };

        private readonly List<double> BOP= new List<double>
        {
            .05, .03, .01, -.01, -.03, -.05
        };

        private readonly List<double> MACD = new List<double>
        {
            0,-1,-2,-3
        };

        private readonly List<double> MOM = new List<double>
        {
            0,-1,-3,-5,-7
        };

        private readonly List<double> RSI = new List<double>
        {
            30,35,40,45,50,55,60,
        };


        public IEnumerable<IEnumerable<T>> SubSetsOf<T>(IEnumerable<T> source)
        {
            if (!source.Any())
                return Enumerable.Repeat(Enumerable.Empty<T>(), 1);

            var element = source.Take(1);

            var haveNots = SubSetsOf(source.Skip(1));
            var haves = haveNots.Select(set => element.Concat(set));

            return haves.Concat(haveNots);
        }

        public void BuildComparison(StockData stock, List<string> combination, string finishedList = "")
        {
            if (combination.Count != 0)
            {
                string current = combination[combination.Count - 1];
                combination.RemoveAt(combination.Count - 1);

                switch (current)
                {
                    case ("RSI"):
                        foreach (var rsi in RSI)
                        {
                            if (stock.RSI <= rsi)
                            {
                                BuildComparison(stock, combination, finishedList + $"RSI{rsi}/");
                            }
                        }
                        break;

                    case ("MOM"):
                        foreach (var mom in MOM)
                        {
                            if (stock.MOM <= mom)
                            {
                                BuildComparison(stock, combination, finishedList + $"MOM{mom}/");
                            }
                        }
                        break;

                    case ("MACD"):
                        foreach (var macd in MACD)
                        {
                            if (stock.MACD <= macd)
                            {
                                BuildComparison(stock, combination, finishedList + $"MACD{macd}/");
                            }
                        }
                        break;

                    case ("BOP"):
                        foreach (var bop in BOP)
                        {
                            if (stock.BOP <= bop)
                            {
                                BuildComparison(stock, combination, finishedList + $"BOP{bop}/");
                            }
                        }
                        break;

                    case ("BBANDS"):
                        foreach (var bband in BBANDS)
                        {
                            if (stock.BBANDS <= bband)
                            {
                                BuildComparison(stock, combination, finishedList + $"BBANDS{bband}/");
                            }
                        }
                        break;

                    case ("ADX"):
                        foreach (var adx in ADX)
                        {
                            if(stock.ADX <= adx)
                            {
                                BuildComparison(stock, combination, finishedList + $"ADX{adx}/");
                            }                          
                        }
                        break;

                    default:
                        break;
                }
            }
            else
            {
                if (stock.Gain > 1.00)
                {
                    AddToSucess(stock, finishedList, 1);
                }
                else
                {
                    AddToSucess(stock, finishedList, 0);
                }
            }
        }

        private void AddToSucess(StockData stock, string finishedList, int succeed)
        {
            finished.Add(finishedList);
            if (successRate.ContainsKey(finishedList))
            {
                if(!successRate[finishedList].ContainsKey(stock.Symbol))
                {
                    successRate[finishedList].Add(stock.Symbol, succeed);
                    if (succeed == 1)
                    {
                        successRate[finishedList]["Success"]++;
                        successRate[finishedList]["Total"]++;
                    }
                    else
                    {
                        successRate[finishedList]["Total"]++;
                    }
                }                
            }
            else
            {
                successRate.Add(finishedList, new Dictionary<string, int>());
                successRate[finishedList].Add("Total", 1);
                if(succeed == 1)
                {
                    successRate[finishedList].Add("Success", 1);
                }
                else
                {
                    successRate[finishedList].Add("Success", 0);
                }
                successRate[finishedList].Add(stock.Symbol, succeed);
            }
        }

        public void SaveComparisons(List<IComparisonResult> comparisonData, string month)
        {
            var headers = "Test Name, Success%, Total, Success "; //sets header string
            var path = $@"..\..\..\Files\Results\{month}.csv";
            using (var file = File.CreateText(path))
            {
                file.WriteLine(headers); //writes headers
                foreach (var test in comparisonData) //writes whole list
                {
                    file.WriteLine(string.Join(",", test.TestName, test.SuccessPercent, test.Total, test.Success));
                }
            }
        }

        public List<IComparisonResult> RetrieveComparisons(string month)
        {
            string[] fileArray = Directory.GetFiles(@"..\..\..\Files\Results", $"{month}.csv", SearchOption.AllDirectories); //gets months fundamentals

            if (fileArray != null)
            {
                var comparisonList = new List<IComparisonResult>();
                foreach (string file in fileArray)
                {
                    var fundamentals = File.ReadLines(file).Select(line => line.Split(',')).ToList(); //grabs csv lines and makes dictionary indexed at symbol
                    fundamentals.RemoveAt(0);
                    foreach(var test in fundamentals)
                    {
                        comparisonList.Add(new ComparisonResult
                        {
                            TestName = test[0],
                            SuccessPercent = double.Parse(test[1]),
                            Total = double.Parse(test[2]),
                            Success = double.Parse(test[3])
                        });
                    }
                }

                return comparisonList;
            }
            else
            {
                return null;
            }
        }

        public Dictionary<string,Dictionary<string,double>> RetrieveAllComparisons()
        {
            string[] fileArray = Directory.GetFiles(@"..\..\..\Files\Results", "*.csv", SearchOption.AllDirectories); //gets months fundamentals


            var testResults = new List<TestResults>();
            var testList = new Dictionary<string, Dictionary<string,double>>();

            foreach (string file in fileArray)
            {
                var month = file.Split('\\')[5];
                var fundamentals = File.ReadLines(file).Select(line => line.Split(',')).ToList(); //grabs csv lines and makes dictionary indexed at symbol
                fundamentals.RemoveAt(0);
                foreach (var test in fundamentals)
                {
                    if(testList.ContainsKey(test[0]))
                    {
                        testList[test[0]].Add(month,double.Parse(test[1]));
                    }
                    else
                    {
                        testList.Add(test[0], new Dictionary<string, double>
                        {
                            { month, double.Parse(test[1]) }
                        });
                    }
                }
            }         

            return testList;

        }
    }
}
