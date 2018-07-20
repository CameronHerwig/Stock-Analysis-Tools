namespace Stock_Data
{
    public interface IStockData 
    {
        string Symbol { get; set; }     
        double Price { get; set; }
        double EarningsGrowth { get; set; }
        double ADX { get; set; }
        double BBANDS { get; set; }
        double BOP { get; set; }
        double MACD { get; set; }
        double MOM { get; set; }
        double RSI { get; set; }
        double Gain { get; set; }
        double ForwardPE { get; set; }
    }
}
