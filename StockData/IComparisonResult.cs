using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Data
{
    public interface IComparisonResult
    {
        string TestName { get; set; }
        double SuccessPercent{ get; set; }
        double Total { get; set; }
        double Success { get; set; }
    }
}
