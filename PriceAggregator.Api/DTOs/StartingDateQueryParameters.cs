using System.ComponentModel.DataAnnotations;

namespace PriceAggregator.Api.DTOs
{
    public class StartingDateQueryParameters
    {
        [Required]
        [Range(2015, int.MaxValue)]
        public int StartYear { get; set; }

        [Required]
        [Range(1, 12)]
        public int StartMonth { get; set; }

        [Required]
        [Range(1, 31)]
        public int StartDay { get; set; }

        [Required]
        [Range(0, 23)]
        public int StartHour { get; set; }
    }
}
