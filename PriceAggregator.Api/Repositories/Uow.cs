using Microsoft.EntityFrameworkCore;
using PriceAggregator.Api.Data;

namespace PriceAggregator.Api.Repositories
{
    public class Uow : IUow
    {
        private readonly PriceAggregatorContext _priceDbContext;

        public Uow(PriceAggregatorContext priceAggregatorContext)
        {
            _priceDbContext = priceAggregatorContext;
        }

        public IPriceRepository PriceRepository => new PriceRepository(_priceDbContext);

        public async Task Complete()
        {
            await _priceDbContext.SaveChangesAsync();
        }
    }
}
