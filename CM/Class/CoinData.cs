using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;


namespace CM
{
    public enum CoinTrend
    {
        Up,     // 0
        Down,    // 1
        Equal  // 2            
    }

    public class CoinDataAll
    {
        public List<CoinData> Items { get; } = new List<CoinData>();
        public bool IsUsed { get; set; }
     
    }

    public class CoinData
    {
        public string Name { get; set; }
        public double CurrentPrice { get; set; }
        public double PreviousPrice { get; set; }
        public string Trend { get; set; }
        public double WarnPercentage { get; set; }

        public double SessionStartPrice { get; set; }
        public double SessionHighPrice { get; set; }
        public double SessionLowPrice { get; set; }
        public double DiffValuta { get; set; }
        public double DiffPercent { get; set; }
        public double RateWhenProfit { get; set; }
        public double RateWhenLost { get; set; }

        public double Open24 { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Last { get; set; }
        public double Volume { get; set; }
        public double VolumeQuote { get; set; }
        public double Bid { get; set; }
        public double BidSize { get; set; }
        public double Ask { get; set; }
        public double AskSize { get; set; }
        public double DiffPercentOpen24 { get; set; }
        public bool IsSelected { get; set; }
    }
}
