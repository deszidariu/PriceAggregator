using PriceAggregator.Api.Models;

namespace PriceAggregator.Api.Services
{
    public interface IExternalSourcePrices
    {
        Task<Price?> GetPriceByHourAsync(CurrencyCode fromCurrency, CurrencyCode toCurrency, DateTime from, DateTime to);
    }
}
