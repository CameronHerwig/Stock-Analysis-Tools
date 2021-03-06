﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Data
{
    public class FutureData : IFutureData, INotifyPropertyChanged
    {
        public string Symbol { get; set; }
        public double Price { get; set; }
        public double EarningsGrowth { get; set; }
        public double ADX { get; set; }
        public double BBANDS { get; set; }
        public double BOP { get; set; }
        public double MACD { get; set; }
        public double MOM { get; set; }
        public double RSI { get; set; }
        public double ForwardPE { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
