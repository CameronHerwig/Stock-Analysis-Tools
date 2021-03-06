﻿using StockData.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Stock_Data
{
    public class FundamentalChooser
    {
        readonly double minimumGain = double.Parse(Settings.Default.MinimumGain);
        readonly string testFolder = Settings.Default.MinimumGain;

        private List<string> finished = new List<string>();

        public Dictionary<string, Dictionary<string, double>> successRate = new Dictionary<string, Dictionary<string, double>>();

        private readonly List<double> ADX = new List<double>
        {
            20, 25, 30, 35, 40, 45, 50
        };

        private readonly List<double> BBANDS = new List<double>
        {
            .07, .06, .05, .04, .03, .02
        };

        private readonly List<double> BOP = new List<double>
        {
            .07, .05, .03, .01, -.01, -.03, -.05, -.07
        };

        private readonly List<double> MACD = new List<double>
        {
            3, 2, 1, 0,-1,-2,-3
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
                            if (stock.ADX <= adx)
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
                if (stock.Gain > minimumGain)
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
                if (!successRate[finishedList].ContainsKey(stock.Symbol))
                {
                    successRate[finishedList].Add(stock.Symbol, succeed);
                    if (succeed == 1)
                    {
                        successRate[finishedList]["Success"]++;
                        successRate[finishedList]["Total"]++;
                        successRate[finishedList]["TotalGain"] += stock.Gain;
                    }
                    else
                    {
                        successRate[finishedList]["Total"]++;
                        successRate[finishedList]["TotalGain"] += stock.Gain;
                    }
                }
            }
            else
            {
                successRate.Add(finishedList, new Dictionary<string, double>());
                successRate[finishedList].Add("Total", 1);
                successRate[finishedList].Add("TotalGain", stock.Gain);
                if (succeed == 1)
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
            var headers = "Test Name, Success%, Total, Success, AvgGain"; //sets header string
            Directory.CreateDirectory($@"..\..\..\Files\Results\{testFolder}\");
            var path = $@"..\..\..\Files\Results\{testFolder}\{month}.csv";
            using (var file = File.CreateText(path))
            {
                file.WriteLine(headers); //writes headers
                foreach (var test in comparisonData) //writes whole list
                {
                    file.WriteLine(string.Join(",", test.TestName, test.SuccessPercent, test.Total, test.Success, test.AvgGain));
                }
            }
        }

        public List<IComparisonResult> RetrieveComparisons(string month)
        {
            string[] fileArray = Directory.GetFiles($@"..\..\..\Files\Results\{testFolder}\", $"{month}.csv", SearchOption.TopDirectoryOnly); //gets months fundamentals

            if (fileArray != null)
            {
                var comparisonList = new List<IComparisonResult>();
                foreach (string file in fileArray)
                {
                    var fundamentals = File.ReadLines(file).Select(line => line.Split(',')).ToList(); //grabs csv lines and makes dictionary indexed at symbol
                    fundamentals.RemoveAt(0);
                    foreach (var test in fundamentals)
                    {
                        comparisonList.Add(new ComparisonResult
                        {
                            TestName = test[0],
                            SuccessPercent = double.Parse(test[1]),
                            Total = double.Parse(test[2]),
                            Success = double.Parse(test[3]),
                            AvgGain = double.Parse(test[4]),
                        });
                    }
                }

                return comparisonList;
            }
            return null;
        }

        public List<ExpandoObject> RetrieveAllComparisonsDynamic(bool keepEmpties)
        {
            DirectoryInfo di = Directory.CreateDirectory($@"..\..\..\Files\Results\{testFolder}\");
            string[] fileArray = Directory.GetFiles($@"..\..\..\Files\Results\{testFolder}\", "*.csv", SearchOption.TopDirectoryOnly); //gets months fundamental

            var testList = new Dictionary<string, ExpandoObject>();

            foreach (string file in fileArray)
            {
                var month = file.Split('\\')[6].Split('.')[0];
                var fundamentals = File.ReadLines(file).Select(line => line.Split(',')).ToList(); //grabs csv lines and makes dictionary indexed at symbol
                fundamentals.RemoveAt(0);

                foreach (var tests in fundamentals)
                {
                    if (testList.ContainsKey(tests[0]))
                    {
                        dynamic expandObject = testList[tests[0]];

                        var testAvg = double.Parse((expandObject as IDictionary<String, object>)["AverageSymbols"].ToString());
                        testAvg += double.Parse(tests[3]);
                        (expandObject as IDictionary<String, object>)["AverageSymbols"] = testAvg;

                        var testGain = double.Parse((expandObject as IDictionary<String, object>)["AverageGain"].ToString());
                        testGain += double.Parse(tests[4]);
                        (expandObject as IDictionary<String, object>)["AverageGain"] = testGain;


                        AddProperty(expandObject, month, double.Parse(tests[1]));
                    }
                    else
                    {
                        dynamic testObject = new ExpandoObject();
                        testObject.TestName = tests[0];
                        testObject.AverageSymbols = double.Parse(tests[3]);
                        testObject.AverageGain = double.Parse(tests[4]);
                        AddProperty(testObject, month, double.Parse(tests[1]));

                        testList.Add(tests[0], testObject);
                    }
                }
            }

            foreach (var test in testList)
            {
                var values = test.Value.ToList();
                var avgSymbols = Math.Round(double.Parse(values[1].Value.ToString()) / (fileArray.Length), 2);
                var avgGain = Math.Round(double.Parse(values[2].Value.ToString()) / (fileArray.Length), 4);
                double TotalSuccess = 0;
                for (int i = 3; i < values.Count; i++)
                {
                    TotalSuccess += double.Parse(values[i].Value.ToString());
                }
                TotalSuccess /= (fileArray.Length);

                ((IDictionary<String, object>)test.Value)["AverageSymbols"] = avgSymbols;
                ((IDictionary<String, object>)test.Value)["AverageGain"] = avgGain;
                AddProperty(test.Value, "SuccessPercent", TotalSuccess);
            }

            var finished = testList.Values.ToList();

            SaveAllComparisonsDynamic(finished);

            if (keepEmpties)
            {
                finished.RemoveAll(c => c.Count() != (fileArray.Length + 4)); //Optionally Remove All Empty Months
            }

            return finished;
        }

        public void SaveAllComparisonsDynamic(List<ExpandoObject> testData)
        {
            var headers = ""; //sets header string
            var headerList = new List<string>();

            foreach (var test in testData)
            {
                var result = (test as IDictionary<String, object>);

                foreach (var field in result)
                {
                    if (!headerList.Contains(field.Key))
                    {
                        headerList.Add(field.Key);
                        headers += $", {field.Key}";
                    }
                }
            }
            headers = headers.Substring(2);

            var path = $@"..\..\..\Files\Results\{testFolder}\Combined\Combined.csv";
            Directory.CreateDirectory($@"..\..\..\Files\Results\{testFolder}\");
            DirectoryInfo di = Directory.CreateDirectory($@"..\..\..\Files\Results\{testFolder}\Combined\");

            try
            {
                using (var file = File.CreateText(path))
                {
                    file.WriteLine(headers); //writes headers
                    foreach (var test in testData) //writes whole list
                    {
                        var result = (test as IDictionary<String, object>);
                        string toWrite = "";
                        foreach (var header in headerList)
                        {
                            if (result.ContainsKey(header))
                            {
                                toWrite += $", {result[header]}";
                            }
                            else
                            {
                                toWrite += ",";
                            }
                        }
                        toWrite = toWrite.Substring(2);
                        file.WriteLine(toWrite);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Combined Data File Must Be Closed!", "Stock_Data:FundamentalChooser:SaveAllComparisons", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }


        //public List<ITestResults> RetrieveAllComparisons()
        //{
        //    DirectoryInfo di = Directory.CreateDirectory($@"..\..\..\Files\Results\{testFolder}\");
        //    string[] fileArray = Directory.GetFiles($@"..\..\..\Files\Results\{testFolder}\", "*.csv", SearchOption.TopDirectoryOnly); //gets months fundamentals

        //    dynamic expando = new ExpandoObject();

        //    var testList = new Dictionary<string, ITestResults>();

        //    foreach (string file in fileArray)
        //    {
        //        var month = file.Split('\\')[6];
        //        var fundamentals = File.ReadLines(file).Select(line => line.Split(',')).ToList(); //grabs csv lines and makes dictionary indexed at symbol
        //        fundamentals.RemoveAt(0);
        //        foreach (var test in fundamentals)
        //        {
        //            if (testList.ContainsKey(test[0]))
        //            {
        //                switch (month)
        //                {
        //                    case "September17.csv":
        //                        testList[test[0]].September17 = double.Parse(test[1]);
        //                        testList[test[0]].AverageSymbols += double.Parse(test[3]);
        //                        testList[test[0]].AverageGain += double.Parse(test[4]);
        //                        break;
        //                    case "October17.csv":
        //                        testList[test[0]].October17 = double.Parse(test[1]);
        //                        testList[test[0]].AverageSymbols += double.Parse(test[3]);
        //                        testList[test[0]].AverageGain += double.Parse(test[4]);
        //                        break;
        //                    case "November17.csv":
        //                        testList[test[0]].November17 = double.Parse(test[1]);
        //                        testList[test[0]].AverageSymbols += double.Parse(test[3]);
        //                        testList[test[0]].AverageGain += double.Parse(test[4]);
        //                        break;
        //                    case "December17.csv":
        //                        testList[test[0]].December17 = double.Parse(test[1]);
        //                        testList[test[0]].AverageSymbols += double.Parse(test[3]);
        //                        testList[test[0]].AverageGain += double.Parse(test[4]);
        //                        break;
        //                    case "Janurary18.csv":
        //                        testList[test[0]].Janurary18 = double.Parse(test[1]);
        //                        testList[test[0]].AverageSymbols += double.Parse(test[3]);
        //                        testList[test[0]].AverageGain += double.Parse(test[4]);
        //                        break;
        //                    case "Feburary18.csv":
        //                        testList[test[0]].Feburary18 = double.Parse(test[1]);
        //                        testList[test[0]].AverageSymbols += double.Parse(test[3]);
        //                        testList[test[0]].AverageGain += double.Parse(test[4]);
        //                        break;
        //                    case "March18.csv":
        //                        testList[test[0]].March18 = double.Parse(test[1]);
        //                        testList[test[0]].AverageSymbols += double.Parse(test[3]);
        //                        testList[test[0]].AverageGain += double.Parse(test[4]);
        //                        break;
        //                }
        //            }
        //            else
        //            {
        //                switch (month)
        //                {
        //                    case "September17.csv":
        //                        testList.Add(test[0], new TestResults { TestName = test[0], September17 = double.Parse(test[1]), AverageSymbols = double.Parse(test[3]), AverageGain = double.Parse(test[4]) });
        //                        break;
        //                    case "October17.csv":
        //                        testList.Add(test[0], new TestResults { TestName = test[0], October17 = double.Parse(test[1]), AverageSymbols = double.Parse(test[3]), AverageGain = double.Parse(test[4]) });
        //                        break;
        //                    case "November17.csv":
        //                        testList.Add(test[0], new TestResults { TestName = test[0], November17 = double.Parse(test[1]), AverageSymbols = double.Parse(test[3]), AverageGain = double.Parse(test[4]) });
        //                        break;
        //                    case "December17.csv":
        //                        testList.Add(test[0], new TestResults { TestName = test[0], December17 = double.Parse(test[1]), AverageSymbols = double.Parse(test[3]), AverageGain = double.Parse(test[4]) });
        //                        break;
        //                    case "Janurary18.csv":
        //                        testList.Add(test[0], new TestResults { TestName = test[0], Janurary18 = double.Parse(test[1]), AverageSymbols = double.Parse(test[3]), AverageGain = double.Parse(test[4]) });
        //                        break;
        //                    case "Feburary18.csv":
        //                        testList.Add(test[0], new TestResults { TestName = test[0], Feburary18 = double.Parse(test[1]), AverageSymbols = double.Parse(test[3]), AverageGain = double.Parse(test[4]) });
        //                        break;
        //                    case "March18.csv":
        //                        testList.Add(test[0], new TestResults { TestName = test[0], March18 = double.Parse(test[1]), AverageSymbols = double.Parse(test[3]), AverageGain = double.Parse(test[4]) });
        //                        break;
        //                }
        //            }
        //        }
        //    }

        //    foreach (var test in testList)
        //    {
        //        test.Value.Average = Math.Round(((test.Value.September17 +
        //            test.Value.October17 +
        //            test.Value.November17 +
        //            test.Value.December17 +
        //            test.Value.Janurary18 +
        //            test.Value.Feburary18 +
        //            test.Value.March18) / fileArray.Length), 4);
        //        test.Value.AverageSymbols = Math.Round((test.Value.AverageSymbols / fileArray.Length), 2);
        //        test.Value.AverageGain = Math.Round((test.Value.AverageGain / fileArray.Length), 4);
        //    }

        //    var returnList = testList.Values.ToList();

        //    SaveAllComparisons(returnList);

        //    return returnList;

        //}

        //public void SaveAllComparisons(List<ITestResults> testData)
        //{
        //    var headers = "Test Name, September17, October17, November17, December17, Janurary18, Feburary18, March18, Average%, Average Symbols, Average Gain"; //sets header string
        //    var path = $@"..\..\..\Files\Results\{testFolder}\Combined\Combined.csv";
        //    DirectoryInfo di = Directory.CreateDirectory($@"..\..\..\Files\Results\{testFolder}\Combined\");

        //    try
        //    {
        //        using (var file = File.CreateText(path))
        //        {
        //            file.WriteLine(headers); //writes headers
        //            foreach (var test in testData) //writes whole list
        //            {
        //                file.WriteLine(string.Join(",", test.TestName, test.September17, test.October17, test.November17, test.December17, test.Janurary18, test.Feburary18, test.Average, test.AverageSymbols, test.AverageGain));
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Combined Data File Must Be Closed!", "Stock_Data:FundamentalChooser:SaveAllComparisons", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //    }
        //}
    }
}
