using System.Collections.Generic;

namespace Stock_Data
{
    public class Dates
    {
        private readonly Dictionary<string, string> ADX = new Dictionary<string, string>
        {
            {"August17", "2017-07-28"},
            {"September17", "2017-09-01"},
            {"October17", "2017-09-29" },
            {"November17", "2017-10-27" },
            {"December17", "2017-12-01" },
            {"Janurary18", "2017-12-29" },
            {"Feburary18", "2018-02-02" },
            {"March18", "2018-03-02" },
            {"April18", "2018-03-29" },
            {"May18", "2018-04-27" },
            {"June18", "2018-06-01" },
            {"July18", "2018-06-29" },
            {"August18", "2018-07-27" }
        };
        private readonly Dictionary<string, string> BBANDS = new Dictionary<string, string>
        {
            {"August17", "2017-07-28"},
            {"September17", "2017-09-01"},
            {"October17", "2017-09-29" },
            {"November17", "2017-10-27" },
            {"December17", "2017-12-01" },
            {"Janurary18", "2017-12-29" },
            {"Feburary18", "2018-02-02" },
            {"March18", "2018-03-02" },
            {"April18", "2018-03-29" },
            {"May18", "2018-04-27" },
            {"June18", "2018-06-01" },
            {"July18", "2018-06-29" },
            {"August18", "2018-07-27" }
        };
        private readonly Dictionary<string, string> BOP = new Dictionary<string, string>
        {
            {"August17", "2017-07-31"},
            {"September17", "2017-08-31"},
            {"October17", "2017-09-29" },
            {"November17", "2017-10-31" },
            {"December17", "2017-11-30" },
            {"Janurary18", "2017-12-29" },
            {"Feburary18", "2018-01-31" },
            {"March18", "2018-02-28" },
            {"April18", "2018-03-29" },
            {"May18", "2018-04-30" },
            {"June18", "2018-05-31" },
            {"July18", "2018-06-29" },
            {"August18", "2018-07-31" }
        };
        private readonly Dictionary<string, List<string>> MOM = new Dictionary<string, List<string>>
        {
            {"August17", new List<string>{"2017-07-24", "2017-07-25", "2017-07-26", "2017-07-27", "2017-07-28"}},
            {"September17", new List<string>{"2017-08-28", "2017-08-29", "2017-08-30", "2017-08-31", "2017-09-01"}},
            {"October17", new List<string>{"2017-09-25", "2017-09-26", "2017-09-27", "2017-09-28", "2017-09-29"}},
            {"November17", new List<string>{"2017-10-23", "2017-10-24", "2017-10-25", "2017-10-26", "2017-10-27"}},
            {"December17", new List<string>{ "2017-11-27", "2017-11-28", "2017-11-29", "2017-11-30", "2017-12-01"}},
            {"Janurary18", new List<string>{ "2017-12-26", "2017-12-27", "2017-12-28", "2017-12-29", "2017-01-02"}},
            {"Feburary18", new List<string>{ "2018-01-29", "2018-01-30", "2018-01-31", "2018-02-01", "2018-02-02"}},
            {"March18", new List<string>{ "2018-02-26", "2018-02-27", "2018-02-28", "2018-03-01", "2018-03-02"}},
            {"April18", new List<string>{ "2018-03-26", "2018-03-27", "2018-03-28", "2018-03-29", "2018-04-02"}},
            {"May18", new List<string>{ "2018-04-25", "2018-04-26", "2018-04-27", "2018-04-30", "2018-05-01"}},
            {"June18", new List<string>{ "2018-05-25", "2018-05-29", "2018-05-30", "2018-05-31", "2018-06-01"}},
            {"July18", new List<string>{ "2018-06-25", "2018-06-26", "2018-06-27", "2018-06-28", "2018-06-29"}},
            {"August18", new List<string>{ "2018-01-29", "2018-01-30", "2018-01-31", "2018-02-01", "2018-02-02"}}
        };
        private readonly Dictionary<string, string> MACD = new Dictionary<string, string>
        {
            {"August17", "2017-07-31"},
            {"September17", "2017-08-31"},
            {"October17", "2017-09-29" },
            {"November17", "2017-10-31" },
            {"December17", "2017-11-30" },
            {"Janurary18", "2017-12-29" },
            {"Feburary18", "2018-01-31" },
            {"March18", "2018-02-28" },
            {"April18", "2018-03-29" },
            {"May18", "2018-04-30" },
            {"June18", "2018-05-31" },
            {"July18", "2018-06-29" },
            {"August18", "2018-07-31" }
        };
        private readonly Dictionary<string, string> RSI = new Dictionary<string, string>
        {
            {"August17", "2017-07-28"},
            {"September17", "2017-09-01"},
            {"October17", "2017-09-29" },
            {"November17", "2017-10-27" },
            {"December17", "2017-12-01" },
            {"Janurary18", "2017-12-29" },
            {"Feburary18", "2018-02-02" },
            {"March18", "2018-03-02" },
            {"April18", "2018-03-29" },
            {"May18", "2018-04-27" },
            {"June18", "2018-06-01" },
            {"July18", "2018-06-29" },
            {"August18", "2018-07-27" }
        };
        private readonly Dictionary<string, string> Price = new Dictionary<string, string>
        {
            {"August17", "2017-07-28"},
            {"September17", "2017-09-01"},
            {"October17", "2017-09-29" },
            {"November17", "2017-10-27" },
            {"December17", "2017-12-01" },
            {"Janurary18", "2017-12-29" },
            {"Feburary18", "2018-02-02" },
            {"March18", "2018-03-02" },
            {"April18", "2018-03-29" },
            {"May18", "2018-04-27" },
            {"June18", "2018-06-01" },
            {"July18", "2018-06-29" },
            {"August18", "2018-07-27" }
        };
        private readonly Dictionary<string, List<string>> Gain = new Dictionary<string, List<string>>
        {
            {"August17", new List<string>{"2017-08-01", "2017-08-31"}},
            {"September17", new List<string>{ "2017-09-01", "2017-09-29"}},
            {"October17", new List<string>{ "2017-10-02", "2017-10-31"}},
            {"November17", new List<string>{ "2017-11-01", "2017-11-30"}},
            {"December17", new List<string>{ "2017-12-01", "2017-12-29"}},
            {"Janurary18", new List<string>{ "2018-01-02", "2018-01-31"}},
            {"Feburary18", new List<string>{ "2018-02-01", "2018-02-28"}},
            {"March18", new List<string>{ "2018-03-01", "2018-03-29" } },
            {"April18", new List<string>{ "2018-04-02", "2018-04-30" } },
            {"May18", new List<string>{ "2018-05-01", "2018-05-31"} },
            {"June18", new List<string>{ "2018-06-01", "2018-06-29" } },
            {"July18", new List<string>{ "2018-07-02", "2018-07-31" } },
            //{"August18", new List<string>{ "2018-02-01", "2018-02-28"} }
        };


        public dynamic GetDate(string fundamental, string month) //Returns the correct date for the month and fundamental passed in
        {
            switch (fundamental)
            {
                case ("ADX"):
                    return ADX[month];
                case ("BBANDS"):
                    return BBANDS[month];
                case ("BOP"):
                    return BOP[month];
                case ("MACD"):
                    return MACD[month];
                case ("MOM"):
                    return MOM[month];
                case ("RSI"):
                    return RSI[month];
                case ("Price"):
                    return Price[month];
                case ("Gain"):
                    return Gain[month];
                default:
                    return null;
            }
        }



    }
}
