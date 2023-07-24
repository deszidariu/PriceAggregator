using Microsoft.Extensions.Configuration;
using Moq.Protected;
using Moq;
using PriceAggregator.Api.Models;
using PriceAggregator.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PriceAggregatorTest.Helper;
using PriceAggregator.Api.Helpers;
using PriceAggregator.Api.Models.BitstampPriceModel;
using System.Text.Json;

namespace PriceAggregatorTest
{
    public class BitstampServiceTests
    {
        [Fact]
        public async void GetPriceByHourTest_Success()
        {
            // Arrange
            var expectedCloseValue = "16556";
            var expectedFromCurrency = "BTC";
            var expectedToCurrency = "USD";
            var expectedStartDateTime = new DateTime(2000, 01, 01);
            var expectedEndDateTime = new DateTime(2009, 01, 01);

            var mockConfSection = new Mock<IConfigurationSection>();
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "bitstamp")]).Returns("https://www.bitstamp.net/api/v2");

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(a => a.GetSection(It.Is<string>(s => s == "ExternalSource"))).Returns(mockConfSection.Object);

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            var bitstampPrice = new Root()
            {
                data = new Data
                {
                    ohlc = new List<Ohlc>
                    {
                        new Ohlc
                        {
                            close = expectedCloseValue,
                            high = "16559",
                            low = "16551",
                            open = "16552",
                            timestamp = expectedStartDateTime.ConvertFromDateToUnixTimeSecondsString(),
                            volume = "123"
                        }
                    },
                    pair = $"{expectedFromCurrency}/{expectedToCurrency}"
                }

            };

            HttpResponseMessage result = new HttpResponseMessage();
            result.StatusCode = System.Net.HttpStatusCode.OK;

            string jsonString = JsonSerializer.Serialize(bitstampPrice);
            result.Content = new StringContent(jsonString);

            handlerMock
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>(
                     "SendAsync",
                     ItExpr.IsAny<HttpRequestMessage>(),
                     ItExpr.IsAny<CancellationToken>()
                    )
                    .Returns(Task.FromResult(result))
                    .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();

            mockHttpClientFactory.Setup(_ => _.CreateClient("bitstamp")).Returns(httpClient);

            var service = new Bitstamp(mockHttpClientFactory.Object, mockConfiguration.Object);

            // Act
            var resultPrice = await service.GetPriceByHourAsync(CurrencyCode.BTC, CurrencyCode.USD, expectedStartDateTime, expectedEndDateTime);

            // Assert
            Assert.Equal(expectedToCurrency, resultPrice?.ToCurrency.ToString());
            Assert.Equal(expectedFromCurrency, resultPrice?.FromCurrency.ToString());
            Assert.Equal(expectedCloseValue, resultPrice?.Close.ToString());
            Assert.Equal(expectedStartDateTime, resultPrice?.StartDateTime);

            handlerMock.VerifyCalls(r => r.RequestUri.Query
            .Contains($"?step=3600&limit=1&start={expectedStartDateTime.ConvertFromDateToUnixTimeSecondsString()}&end={expectedEndDateTime.ConvertFromDateToUnixTimeSecondsString()}"));
        }
    }
}
