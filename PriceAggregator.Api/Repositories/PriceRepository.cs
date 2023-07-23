using Microsoft.EntityFrameworkCore;
using PriceAggregator.Api.Data;
using PriceAggregator.Api.Models;

namespace PriceAggregator.Api.Repositories
{
    public class PriceRepository : IPriceRepository
    {
        private readonly PriceAggregatorContext dbContext;

        public PriceRepository(PriceAggregatorContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Price> AddPriceAsync(Price price)
        {
            var existingPrice = await this.GetPriceByDateTimeAsync(price.StartDateTime, price.FromCurrency, price.ToCurrency);
           
            if (existingPrice == null)
            {
                await dbContext.Prices.AddAsync(price);
                return price;
            }
            
            return existingPrice;
        }

        public async Task<IList<Price>> GetAllPricesAsync()
        {
            return await dbContext.Prices.OrderByDescending(x => x.StartDateTime).ToListAsync();
        }

        public async Task<List<Price>> GetAllPricesBetweenTwoDateTimesAsync(DateTime startingdate, DateTime endingdate, CurrencyCode from, CurrencyCode to)
        {
            return await dbContext.Prices.Where(x => x.StartDateTime >= startingdate && x.StartDateTime <= endingdate && x.FromCurrency == from && x.ToCurrency == to).OrderByDescending(x => x.StartDateTime).ToListAsync();
        }

        public async Task<Price?> GetPriceByDateTimeAsync(DateTime datetime, CurrencyCode from, CurrencyCode to)
        {
            return await dbContext.Prices.FirstOrDefaultAsync(x => x.StartDateTime == datetime && x.FromCurrency == from && x.ToCurrency == to);
        }
    }
}
