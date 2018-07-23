using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Data
{
    class ComparisonResult : IComparisonResult
    {
      public string TestName { get; set; }
      public double SuccessPercent{ get; set; }
      public double Total { get; set; }
      public double Success { get; set; }
    }
}