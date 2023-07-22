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
        public float Close { get; set; }
        [Column("timestamp")]
        public string Timestamp { get; set; }
        [Column("fromcurrency")]
        public CurrencyCode FromCurrency { get; set; }
        [Column("tocurrency")]
        public CurrencyCode ToCurrency { get; set; }
    }
}
