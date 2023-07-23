namespace PriceAggregator.Api.Models.BitstampPriceModel
{
    public class Data
    {
        public List<Ohlc> ohlc { get; set; }
        public string pair { get; set; }
    }

    public class Ohlc
    {
        public string close { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string open { get; set; }
        public string timestamp { get; set; }
        public string volume { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
    }
}
