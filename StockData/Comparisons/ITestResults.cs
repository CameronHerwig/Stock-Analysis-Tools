using System.Collections.Generic;

namespace Stock_Data
{
    public interface ITestResults
    {
        string TestName { get; set; }
        double September17 { get; set; }
        double October17 { get; set; }
        double November17 { get; set; }
        double December17 { get; set; }
        double Janurary18 { get; set; }
        double Feburary18 { get; set; }
        //double March18 { get; set; }
        double Average { get; set; }
        double AverageSymbols { get; set; }
    }
}
