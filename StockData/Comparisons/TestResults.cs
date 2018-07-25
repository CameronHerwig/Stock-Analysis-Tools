using System.Collections.Generic;

namespace Stock_Data
{
    public class TestResults : ITestResults
    {
        public string TestName { get; set; }
        public double September17 { get; set; }
        public double October17 { get; set; }
        public double November17 { get; set; }
        public double December17 { get; set; }
        public double Janurary18 { get; set; }
        public double Feburary18 { get; set; }
        //public double March18 { get; set; }
        public double Average { get; set; }
        public double AverageSymbols { get; set; }
    }
}
