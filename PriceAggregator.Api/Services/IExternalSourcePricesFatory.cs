namespace PriceAggregator.Api.Services
{
    public interface IExternalSourcePricesFatory
    {
        IExternalSourcePrices GetInstance(Type service);
    }
}
