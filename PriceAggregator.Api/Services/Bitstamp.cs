using Microsoft.AspNetCore.WebUtilities;
using PriceAggregator.Api.Helpers;
using PriceAggregator.Api.Models;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;

namespace PriceAggregator.Api.Services
{
    public class Bitstamp : IExternalSourcePrices
    {
        private const string EXTERNAL_SOURCE_SECTION = "ExternalSource";
        private const string BITSTAMP_SECTION = "bitstamp";
        private const string START_QUERYSTRING = "start";
        private const string END_QUERYSTRING = "end";
        private const string LIMIT_QUERYSTRING = "limit";
        private const string STEP_QUERYSTRING = "step";
        private int hour = 1;

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public Bitstamp(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Price?> GetPriceByHourAsync(CurrencyCode fromCurrency, CurrencyCode toCurrency, DateTime from, DateTime to)
        {
            var bitstampRequestUrl = this.GenerateUrlRequest(fromCurrency, toCurrency, from, to);

            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            bitstampRequestUrl);

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream =
                    await httpResponseMessage.Content.ReadAsStringAsync();

                var bitstampPriceModel =
                JsonSerializer.Deserialize<Models.BitstampPriceModel.Root>(contentStream);

                if (bitstampPriceModel != null)
                {

                    return new Price
                    {
                        Close = decimal.Parse(bitstampPriceModel.data.ohlc[0].close),
                        StartDateTime = from,
                        FromCurrency = fromCurrency,
                        ToCurrency = toCurrency
                    };
                }
            }

            return null;
        }

        private string GenerateUrlRequest(CurrencyCode fromCurrency, CurrencyCode toCurrency, DateTime from, DateTime to)
        {
            var bitstampSourceEndPoint = _configuration
                                .GetSection(EXTERNAL_SOURCE_SECTION)
                                .GetSection(BITSTAMP_SECTION);

            var bitfinexRequestUrl = QueryHelpers.AddQueryString
                                    ($"{bitstampSourceEndPoint.Value}/ohlc/{fromCurrency.ToString().ToLower()}{toCurrency.ToString().ToLower()}/",
                                     STEP_QUERYSTRING,
                                     (hour * 60 * 60).ToString());

            bitfinexRequestUrl = QueryHelpers.AddQueryString(bitfinexRequestUrl,
                                 LIMIT_QUERYSTRING,
                                 "1");

            bitfinexRequestUrl = QueryHelpers.AddQueryString(bitfinexRequestUrl,
                                 START_QUERYSTRING,
                                 from.ConvertFromDateToUnixTimeSecondsString());

            bitfinexRequestUrl = QueryHelpers.AddQueryString(bitfinexRequestUrl,
                                 END_QUERYSTRING,
                                 to.ConvertFromDateToUnixTimeSecondsString());

            return bitfinexRequestUrl;
        }
    }
}
