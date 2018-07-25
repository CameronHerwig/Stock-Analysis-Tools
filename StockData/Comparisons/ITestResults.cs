using System.Collections.Generic;

namespace Stock_Data
{
    public interface ITestResults
    {
        string TestName { get; set; }
        Dictionary<string,double> TestResult { get; set; }
    }
}
