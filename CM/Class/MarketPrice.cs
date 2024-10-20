using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using System.Globalization;
using CM.Class;

namespace CM
{
    class MarketPrice
    {
        private dynamic JsonObjSettings { get; set; }

        private string UrlCurrentTickerPrice { get; set; } // "https://api.bitvavo.com/v2/ticker/price"

        private string Url24hTickerPrice { get; set; } // "https://api.bitvavo.com/v2/ticker/24h"

        public Dictionary<string, string> CurrentTickerPrice = new();  // The CURRENT price of all the coins
        public List<string> CoinNames = new(); //Used in configform

        public List<string> SelectedCoinName = new();  // List with the selected coin names
        CoinData aSelectedCoin;

        private CoinDataAll AllCoinData;

        public double WarnPercentage { get; set; }

        private string DecimalSeperator { get; set; }

        public double SessionHighPrice { get; set; }

        public double SessionLowPrice { get; set; }

        public MarketPrice()
        {
            // Configure form needs this constructor
            this.LoadSettings();
            this.UrlCurrentTickerPrice = this.JsonObjSettings.AppParam[0].Url1;
            this.Url24hTickerPrice = this.JsonObjSettings.AppParam[0].Url2;
        }

        public MarketPrice(CoinDataAll AllCoinData)
        {
            this.LoadSettings();
            this.UrlCurrentTickerPrice = this.JsonObjSettings.AppParam[0].Url1;
            this.Url24hTickerPrice = this.JsonObjSettings.AppParam[0].Url2;

            this.CreateSelectedCoinList();
            this.aSelectedCoin = new CoinData();
            this.AllCoinData = AllCoinData;
            this.DecimalSeperator = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;  // Move to a static class double code
        }

        private void LoadSettings()
        {
            using SettingsManager Set = new();
            Set.LoadSettings();
            this.JsonObjSettings = Set.JsonObjSettings;
        }

        private void CreateSelectedCoinList()
        {
            if (StartSession.CheckForInternetConnection())  // First check if there is an active internet connection
            {
                ApplicationDatabase CoinNames = new();
                List<string> AllSelectedCoinNames = CoinNames.GetSelectedCoinNames();

                if (AllSelectedCoinNames != null)
                {
                    foreach (string aCoin in AllSelectedCoinNames)
                    {
                        this.SelectedCoinName.Add(aCoin);
                    }
                }
            }
        }

        public void GetCurrentPriceData()
        {
            this.GetCurrentTickerPrice();
            this.Get24hTickerPrice();
        }

        private void GetCurrentTickerPrice()
        {
            try
            {
                // this avoids the error: SecureChannelFailure
                // ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.UrlCurrentTickerPrice);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader sr = new(response.GetResponseStream());
                string MarketPriceData = sr.ReadToEnd();    // Get the current price of all the coins
                sr.Close();

                this.DeserializeCurrentTickerData(MarketPriceData);  // Deserialize the json data
            }
            catch (Exception ex)
            {
                Logging.WriteToLogError("Fout bij het lezen van de huidige ticker data. (Actuele prijs).");
                Logging.WriteToLogError(string.Format("url: {0}", this.UrlCurrentTickerPrice));
                Logging.WriteToLogError(ex.Message);
                if (CmDebugMode.DebugMode)
                {
                    Logging.WriteToLogDebug(ex.ToString());
                }

                MessageBox.Show(
                    "Onverwachte fout opgetreden." + Environment.NewLine +
                    "Melding:" + Environment.NewLine +
                    ex.Message,
                    "Fout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void DeserializeCurrentTickerData(string CurrentMarketPriceData)
        {   // Deserialize json            
            this.CurrentTickerPrice.Clear();  //Clear the previous market-price data

            List<CurrentTickerMarketPrice> results = JsonSerializer.Deserialize<List<CurrentTickerMarketPrice>>(CurrentMarketPriceData);

            foreach (var tickerMarket in results)
            {
                if (!this.AllCoinData.IsUsed)  //First run
                {
                    if (tickerMarket.Price != null)
                    {
                        CoinData aCoin = new();
                        this.CurrentTickerPrice.Add(tickerMarket.Market, tickerMarket.Price);  //A dictionary with market(coin) name + curent price
                                                                                               //
                        aCoin.Name = tickerMarket.Market;

                        aCoin.CurrentPrice = Convert.ToDouble(tickerMarket.Price.Replace(".", this.DecimalSeperator));


                        aCoin.SessionStartPrice = aCoin.CurrentPrice;
                        aCoin.Trend = CoinTrend.Equal.ToString();
                        aCoin.WarnPercentage = this.WarnPercentage;
                        aCoin.DiffValuta = DiffInValuta(aCoin.SessionStartPrice, aCoin.CurrentPrice);
                        aCoin.DiffPercent = DiffInPerc(aCoin.SessionStartPrice, aCoin.CurrentPrice);
                        aCoin.RateWhenProfit = RateWhenProfit(aCoin.SessionStartPrice, aCoin.CurrentPrice, aCoin.WarnPercentage);
                        aCoin.RateWhenLost = RateWhenLost(aCoin.SessionStartPrice, aCoin.CurrentPrice, aCoin.WarnPercentage);
                        aCoin.SessionHighPrice = aCoin.CurrentPrice;
                        aCoin.SessionLowPrice = aCoin.CurrentPrice;
                        aCoin.PreviousPrice = aCoin.CurrentPrice;
                        aCoin.DiffPercentOpen24 = 0;

                        aCoin.IsSelected = false;
                        foreach (string MarketName in this.SelectedCoinName)  // Is it a selected coin or not?
                        {
                            if (tickerMarket.Market == MarketName)
                            {
                                aCoin.IsSelected = true;
                            }
                        }

                        this.AllCoinData.Items.Add(aCoin);
                    }
                }
                else  // AllCoinData excist, the data will only be replaced
                {
                    this.CurrentTickerPrice.Add(tickerMarket.Market, tickerMarket.Price);  // A dictionary with market(coin) name + curent price

                    foreach (CoinData aCoin in this.AllCoinData.Items)  //CoinDataAll
                    {
                        if (aCoin.Name == tickerMarket.Market)
                        {
                            aCoin.PreviousPrice = aCoin.CurrentPrice;
                            aCoin.CurrentPrice = double.Parse(tickerMarket.Price, System.Globalization.CultureInfo.InvariantCulture);
                            if (aCoin.CurrentPrice == aCoin.PreviousPrice)
                            {
                                aCoin.Trend = CoinTrend.Equal.ToString();
                            }
                            else if (aCoin.CurrentPrice > aCoin.PreviousPrice)
                            {
                                aCoin.Trend = CoinTrend.Up.ToString();
                            }
                            else if (aCoin.CurrentPrice < aCoin.PreviousPrice)
                            {
                                aCoin.Trend = CoinTrend.Down.ToString();
                            }
                            else
                            {
                                aCoin.Trend = "?";
                            }

                            aCoin.WarnPercentage = this.WarnPercentage;  // Can change during the session. So refresh it everytime.
                            aCoin.DiffValuta = DiffInValuta(aCoin.SessionStartPrice, aCoin.CurrentPrice);
                            aCoin.DiffPercent = DiffInPerc(aCoin.SessionStartPrice, aCoin.CurrentPrice);
                            aCoin.RateWhenProfit = RateWhenProfit(aCoin.SessionStartPrice, aCoin.CurrentPrice, aCoin.WarnPercentage);
                            aCoin.RateWhenLost = RateWhenLost(aCoin.SessionStartPrice, aCoin.CurrentPrice, aCoin.WarnPercentage);

                            if (aCoin.CurrentPrice < aCoin.SessionLowPrice)
                            {
                                aCoin.SessionLowPrice = aCoin.CurrentPrice;
                            }
                            else
                            {
                                aCoin.SessionLowPrice = aCoin.SessionLowPrice;
                            }

                            if (aCoin.CurrentPrice > aCoin.SessionHighPrice)
                            {
                                aCoin.SessionHighPrice = aCoin.CurrentPrice;
                            }
                            else
                            {
                                aCoin.SessionHighPrice = aCoin.SessionHighPrice;
                            }
                            aCoin.DiffPercentOpen24 = DiffInPerc(aCoin.Open24, aCoin.CurrentPrice);
                        }
                    }
                }
            }

            this.AllCoinData.IsUsed = true;
        }

        private static double DiffInValuta(double StartPrice, double CurrentPrice)
        {
            if (StartPrice != 0 && CurrentPrice != 0)
            {
                return CurrentPrice - StartPrice;
            }
            else
            {
                return 0;
            }
        }

        private static double DiffInPerc(double StartPrice, double CurrentPrice)
        {
            if (StartPrice != 0 && CurrentPrice != 0)
            {
                return Math.Round((CurrentPrice - StartPrice) / (StartPrice / Convert.ToDouble(100)), 2);
            }
            else
            {
                return 0;
            }
        }

        public static double RateWhenProfit(double StartPrice, double CurrentPrice, double WarnPercentage)
        {
            if (StartPrice != 0 && CurrentPrice != 0)
            {
                double Step1 = (StartPrice / Convert.ToDouble(100));
                double Step2 = Step1 * WarnPercentage;

                return Math.Round(StartPrice + Step2,2);
            }
            else
            {
                return 0;
            }
        }
        public static double RateWhenLost(double StartPrice, double CurrentPrice, double WarnPercentage)
        {
            if (StartPrice != 0 && CurrentPrice != 0)
            {
                double Step1 = (StartPrice / Convert.ToDouble(100));
                double Step2 = Step1 * WarnPercentage;

                return Math.Round(StartPrice - Step2,2);
            }
            else
            {
                return 0;
            }
        }

        public void SaveAllCoinData(CoinDataAll AllCoinData)   //Save the coin data to a SQlite datbabase
        {
            ApplicationDatabase SaveCoindata = new();
            SaveCoindata.SaveCoinData(AllCoinData);
            SaveCoindata.Dispose();
        }


        #region 24 hour data

        private void Get24hTickerPrice()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.Url24hTickerPrice);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader sr = new (response.GetResponseStream());
                string DayMarketPriceData = sr.ReadToEnd();
                sr.Close();

                this.Deserialize24hTickerData(DayMarketPriceData);  // Deserialize the json data
            }
            catch (Exception ex)
            {
                Logging.WriteToLogError("Fout bij het lezen van de 24 uur's ticker data. (24 uur prijs).");
                Logging.WriteToLogError(string.Format("url: {0}", this.Url24hTickerPrice));
                Logging.WriteToLogError(ex.Message);
                if (CmDebugMode.DebugMode)
                {
                    Logging.WriteToLogDebug(ex.ToString());
                }
            }
        }

        private void Deserialize24hTickerData(string DayMarketPriceData)
        {
            List<DayhTickerMarketPrice> results = JsonSerializer.Deserialize<List<DayhTickerMarketPrice>>(DayMarketPriceData);

            foreach (var tickerMarket in results)
            {
                foreach (CoinData aCoin in this.AllCoinData.Items)   // aCoin allready exist here. Loop through and fill
                {
                    if (aCoin.Name == tickerMarket.Market) // MarketName)
                    {

                        if (tickerMarket.Open != null) { aCoin.Open24 = Convert.ToDouble(tickerMarket.Open.Replace(".", this.DecimalSeperator)); } else { aCoin.Open24 = 0; }
                        if (tickerMarket.High != null) { aCoin.High = Convert.ToDouble(tickerMarket.High.Replace(".", this.DecimalSeperator)); } else { aCoin.High = 0; }
                        if (tickerMarket.Low != null) { aCoin.Low = Convert.ToDouble(tickerMarket.Low.Replace(".", this.DecimalSeperator)); } else { aCoin.Low = 0; }
                        if (tickerMarket.Last != null) { aCoin.Last = Convert.ToDouble(tickerMarket.Last.Replace(".", this.DecimalSeperator)); } else { aCoin.Last = 0; }
                        if (tickerMarket.Volume != null) { aCoin.Volume = Convert.ToDouble(tickerMarket.Volume.Replace(".", this.DecimalSeperator)); } else { aCoin.Volume = 0; }
                        if (tickerMarket.VolumeQuote != null) { aCoin.VolumeQuote = Convert.ToDouble(tickerMarket.VolumeQuote.Replace(".", this.DecimalSeperator)); } else { aCoin.VolumeQuote = 0; }
                        if (tickerMarket.Bid != null) { aCoin.Bid = Convert.ToDouble(tickerMarket.Bid.Replace(".", this.DecimalSeperator)); } else { aCoin.Bid = 0; }
                        if (tickerMarket.BidSize != null) { aCoin.BidSize = Convert.ToDouble(tickerMarket.BidSize.Replace(".", this.DecimalSeperator)); } else { aCoin.BidSize = 0; }
                        if (tickerMarket.Ask != null) { aCoin.Ask = Convert.ToDouble(tickerMarket.Ask.Replace(".", this.DecimalSeperator)); } else { aCoin.Ask = 0; }
                        if (tickerMarket.AskSize != null) { aCoin.AskSize = Convert.ToDouble(tickerMarket.AskSize.Replace(".", this.DecimalSeperator)); } else { aCoin.AskSize = 0; }

                        // dayTickerPrice.Timestamp = tickerMarket.Timestamp;    // Fault with deserialize
                    }
                }
            }
        }

        #region used in the configure form

        public void GetAllCoinNames()
        {
            try
            {
                //this avoids the error: SecureChannelFailure
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.UrlCurrentTickerPrice);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader sr = new(response.GetResponseStream());
                string MarketPriceData = sr.ReadToEnd();    // Get the current price of all the coins
                sr.Close();

                this.DeserializeGetAllCoinNames(MarketPriceData);  // Deserialize the json data
            }
            catch (Exception ex)
            {
                Logging.WriteToLogError("Fout bij het lezen van de huidige ticker data. (Actuele prijs).");
                Logging.WriteToLogError(string.Format("url: {0}", this.UrlCurrentTickerPrice));
                Logging.WriteToLogError(ex.Message);
                if (CmDebugMode.DebugMode)
                {
                    Logging.WriteToLogDebug(ex.ToString());
                }

                MessageBox.Show(
                    "Onverwachte fout opgetreden." + Environment.NewLine +
                    "Melding:" + Environment.NewLine +
                    ex.Message,
                    "Fout",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void DeserializeGetAllCoinNames(string CurrentMarketPriceData)
        {
            this.CurrentTickerPrice.Clear();  // Clear the previous market-price data

            List<CurrentTickerMarketPrice> results = JsonSerializer.Deserialize<List<CurrentTickerMarketPrice>>(CurrentMarketPriceData);

            foreach (var tickerMarket in results)
            {
                this.CoinNames.Add(tickerMarket.Market); // Create a list with just the coin names
            }
        }
        #endregion used in the configure form




        public class DayTickerPrice
        {
            public string Market { get; set; }

            public string Open { get; set; }

            public string High { get; set; }

            public string Low { get; set; }

            public string Last { get; set; }

            public string Volume { get; set; }

            public string VolumeQuote { get; set; }

            public string Bid { get; set; }

            public string BidSize { get; set; }

            public string Ask { get; set; }

            public string AskSize { get; set; }
            // public string Timestamp { get; set; }
        }

        private class DayhTickerMarketPrice  // 24hour market
        {
            [JsonPropertyName("market")]
            public string Market { get; set; }

            [JsonPropertyName("open")]
            public string Open { get; set; }

            [JsonPropertyName("high")]
            public string High { get; set; }

            [JsonPropertyName("low")]
            public string Low { get; set; }

            [JsonPropertyName("last")]
            public string Last { get; set; }

            [JsonPropertyName("volume")]
            public string Volume { get; set; }

            [JsonPropertyName("volumeQuote")]
            public string VolumeQuote { get; set; }

            [JsonPropertyName("bid")]
            public string Bid { get; set; }

            [JsonPropertyName("bidSize")]
            public string BidSize { get; set; }

            [JsonPropertyName("ask")]
            public string Ask { get; set; }

            [JsonPropertyName("askSize")]
            public string AskSize { get; set; }

            //[JsonPropertyName("timestamp")]           // Fault with deserialize
            //public string Timestamp { get; set; }

        }
        
        #endregion 24 hour data

        private class CurrentTickerMarketPrice
        {
            [JsonPropertyName("market")]
            public string Market { get; set; }

            [JsonPropertyName("price")]
            public string Price { get; set; }

            //why the: [JsonPropertyName("market")] before : public string Market { get; set; } 
            //This means the property in json is all lower case while when creating a property in a class the first character should be uppercase.
            //If we use Market as a property name the deserialization will fail as it's case sensitive so to keep with C# naming conventions the attribute is used.
        }
    }
}
