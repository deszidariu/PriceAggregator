using PriceAggregator.Api.Models;

namespace PriceAggregator.Api.Services
{
    public interface IBitfinex
    {
        Task<Price?> GetPriceByHour(CurrencyCode fromCurrency, CurrencyCode toCurrency, DateTime from, DateTime to);
    }
}
