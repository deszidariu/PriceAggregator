using System.ComponentModel.DataAnnotations;

namespace PriceAggregator.Api.DTOs
{
    public class DateQueryParameters
    {
        [Required]
        [Range(2015, int.MaxValue)]
        public int Year { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        [Range(1, 31)]
        public int Day { get; set; }

        [Required]
        [Range(0, 23)]
        public int Hour { get; set; }
    }
}
