using System.ComponentModel.DataAnnotations;

namespace PriceAggregator.Api.DTOs
{
    public class EndingDateQueryParameters
    {
        [Required]
        [Range(2015, int.MaxValue)]
        public int EndYear { get; set; }

        [Required]
        [Range(1, 12)]
        public int EndMonth { get; set; }

        [Required]
        [Range(1, 31)]
        public int EndDay { get; set; }

        [Required]
        [Range(0, 23)]
        public int EndHour { get; set; }
    }
}
