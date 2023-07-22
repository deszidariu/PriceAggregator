using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceAggregator.Api.Models
{

    [Table("prices")]
    public class Price
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("close")]
        public string Close { get; set; }
        [Column("high")]
        public string High { get; set; }
        [Column("low")]
        public string Low { get; set; }
        [Column("open")]
        public string Open { get; set; }
        [Column("timestamp")]
        public string Timestamp { get; set; }
        [Column("volume")]
        public string Volume { get; set; }
    }
}
