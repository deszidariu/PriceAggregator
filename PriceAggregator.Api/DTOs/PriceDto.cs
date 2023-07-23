namespace PriceAggregator.Api.DTOs
{
    public class PriceDto
    {
        public string Price { get; set; }
        public DateTime DateTime { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
