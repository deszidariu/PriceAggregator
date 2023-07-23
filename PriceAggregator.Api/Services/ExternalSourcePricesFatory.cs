using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PriceAggregator.Api.Services
{
    public class ExternalSourcePricesFatory : IExternalSourcePricesFatory
    {
        private const string BITFINEX = "Bitfinex";
        private const string BITSTAMP = "Bitstamp";

        public readonly IEnumerable<IExternalSourcePrices> services;

        public ExternalSourcePricesFatory(IEnumerable<IExternalSourcePrices> services)
        {
            this.services = services;
        }

        public IExternalSourcePrices GetInstance(Type service)
        {
            return service.Name switch
            {
                BITFINEX => this.GetService(service),
                BITSTAMP => this.GetService(service),
                _ => throw new InvalidOperationException(),
            };
        }

        public IExternalSourcePrices GetService(Type type)
        {
            return this.services.FirstOrDefault(x => x.GetType() == type)!;
        }
    }
}
