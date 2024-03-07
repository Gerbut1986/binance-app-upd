namespace BinanceOptionsApp.Proccessors
{
    public interface IProcessorInput
    {
        double Bid { get; set; }
        double Ask { get; set; }
        double Point { get; set; }
        System.DateTime Time { get; set; }
    }
}
