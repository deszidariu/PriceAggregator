using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using PriceAggregator.Api.Helpers;
using PriceAggregator.Api.Models;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;

namespace PriceAggregator.Api.Services
{
    public class Bitfinex : IExternalSourcePrices
    {
        private const string EXTERNAL_SOURCE_SECTION = "ExternalSource";
        private const string BITFINEX_SECTION = "bitfinex";
        private const string START_QUERYSTRING = "start";
        private const string END_QUERYSTRING = "end";
        private const string LIMIT_QUERYSTRING = "limit";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration configuration;

        public Bitfinex(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }

        public async Task<Price?> GetPriceByHourAsync(CurrencyCode fromCurrency, CurrencyCode toCurrency, DateTime from, DateTime to)
        {
            var bitfinexRequestUrl = this.GenerateUrlRequest(fromCurrency, toCurrency, from, to); 

            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            bitfinexRequestUrl);

            var httpClient = _httpClientFactory.CreateClient("bitfinex");
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream =
                    await httpResponseMessage.Content.ReadAsStringAsync();

                return ConvertBifinexResultToPriceModel(contentStream, fromCurrency, toCurrency, from);
            }

            return null;
        }

        private Price? ConvertBifinexResultToPriceModel(string contentStream, CurrencyCode fromCurrency, CurrencyCode toCurrency, DateTime from)
        {
            var listOfFildsValue = contentStream.Split(',');

            return new Price
            {
                Close = decimal.Parse(listOfFildsValue[2]),
                StartDateTime = from,
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency
            };
        }

        private string GenerateUrlRequest(CurrencyCode fromCurrency, CurrencyCode toCurrency, DateTime from, DateTime to)
        {
            var bitfinexSourceEndPoint = configuration
                                .GetSection(EXTERNAL_SOURCE_SECTION)?[BITFINEX_SECTION];

            var bitfinexRequestUrl = QueryHelpers.AddQueryString
                                    ($"{bitfinexSourceEndPoint}/candles/trade:1h:t{fromCurrency}{toCurrency}/hist",
                                     START_QUERYSTRING,
                                     from.ConvertFromDateToUnixTimeMillisecondsString());

            bitfinexRequestUrl = QueryHelpers.AddQueryString(bitfinexRequestUrl,
                                 END_QUERYSTRING,
                                 to.ConvertFromDateToUnixTimeMillisecondsString());

            bitfinexRequestUrl = QueryHelpers.AddQueryString(bitfinexRequestUrl,
                                 LIMIT_QUERYSTRING,
                                 "1");

            return bitfinexRequestUrl;
        }
    }
}
