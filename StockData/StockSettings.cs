using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockData
{
    public class StockSettings
    {
        public void SaveSettings(string APIKey = null, string MinimumGain = null, string MinimumDelay = null, string MinimumGrowth = null, string MinimumPrice = null)
        {
            if(APIKey != null)
            {
                Properties.Settings.Default.APIKey = APIKey;
            }
            if(MinimumGain != null)
            {
                Properties.Settings.Default.MinimumGain = MinimumGain;
            }
            if(MinimumDelay != null)
            {
                Properties.Settings.Default.APIDelay = MinimumDelay;
            }
            if(MinimumGrowth != null)
            {
                Properties.Settings.Default.MinimumGrowth = MinimumGrowth;
            }
            if(MinimumPrice != null)
            {
                Properties.Settings.Default.MinimumPrice = MinimumPrice;
            }
        }
    }
}                            
