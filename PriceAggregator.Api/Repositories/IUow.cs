namespace PriceAggregator.Api.Repositories
{
    public interface IUow
    {
        IPriceRepository PriceRepository { get; }
        Task Complete();
    }
}
