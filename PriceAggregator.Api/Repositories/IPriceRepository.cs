using PriceAggregator.Api.Models;

namespace PriceAggregator.Api.Repositories
{
    public interface IPriceRepository
    {
        Task<Price?> GetPriceByDateTimeAsync(DateTime timestamp, CurrencyCode form, CurrencyCode to);
        Task<List<Price>> GetAllPricesBetweenTwoDateTimesAsync(DateTime startingTimestamp, DateTime endingTimestamp, CurrencyCode from, CurrencyCode to);
        Task<Price> AddPriceAsync(Price price);

        Task<IList<Price>> GetAllPricesAsync();
    }
}
