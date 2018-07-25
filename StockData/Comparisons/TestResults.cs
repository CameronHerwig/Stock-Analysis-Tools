using System.Collections.Generic;

namespace Stock_Data
{
    class TestResults : ITestResults
    {
        public string TestName { get; set; }
        public Dictionary<string, double> TestResult { get; set; }
    }
}
