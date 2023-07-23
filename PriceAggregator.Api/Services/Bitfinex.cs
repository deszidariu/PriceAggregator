using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
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

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream =
                    await httpResponseMessage.Content.ReadAsStringAsync();

                return ConvertBifinexResultToPriceModel(contentStream, fromCurrency, toCurrency);
            }

            return null;
        }

        private Price? ConvertBifinexResultToPriceModel(string contentStream, CurrencyCode fromCurrency, CurrencyCode toCurrency)
        {
            var listOfFildsValue = contentStream.Split(',');

            return new Price
            {
                Close = decimal.Parse(listOfFildsValue[2]).ToString("N", CultureInfo.CurrentCulture),
                Timestamp = listOfFildsValue[0].Substring(2, listOfFildsValue[0].Length - 3),
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency
            };
        }

        private string GenerateUrlRequest(CurrencyCode fromCurrency, CurrencyCode toCurrency, DateTime from, DateTime to)
        {
            var unixTimeFrom = new DateTimeOffset(from);
            var unixTimeTo = new DateTimeOffset(to);

            var bitfinexSourceEndPoint = configuration
                                .GetSection(EXTERNAL_SOURCE_SECTION)
                                .GetSection(BITFINEX_SECTION);

            var bitfinexRequestUrl = QueryHelpers.AddQueryString
                                    ($"{bitfinexSourceEndPoint.Value}/candles/trade:1h:t{fromCurrency}{toCurrency}/hist",
                                     START_QUERYSTRING,
                                     unixTimeFrom.ToUnixTimeMilliseconds().ToString());

            bitfinexRequestUrl = QueryHelpers.AddQueryString(bitfinexRequestUrl,
                                 END_QUERYSTRING,
                                 unixTimeTo.ToUnixTimeMilliseconds().ToString());

            bitfinexRequestUrl = QueryHelpers.AddQueryString(bitfinexRequestUrl,
                                 LIMIT_QUERYSTRING,
                                 "1");

            return bitfinexRequestUrl;
        }
    }
}
