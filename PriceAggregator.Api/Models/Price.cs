using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceAggregator.Api.Models
{

    [Table("prices")]
    public class Price
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("close")]
        public decimal Close { get; set; }
        [Column("startdatetime")]
        public DateTime StartDateTime { get; set; }
        [Column("fromcurrency")]
        public CurrencyCode FromCurrency { get; set; }
        [Column("tocurrency")]
        public CurrencyCode ToCurrency { get; set; }
    }
}
