using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using PriceAggregator.Api.Helpers;
using PriceAggregator.Api.Models;
using PriceAggregator.Api.Services;
using PriceAggregatorTest.Helper;
using System.Net.Http;

namespace PriceAggregatorTest
{
    public class BitfinexServiceTests
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
            mockConfSection.SetupGet(m => m[It.Is<string>(s => s == "bitfinex")]).Returns("https://api-pub.bitfinex.com/v2");

            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(a => a.GetSection(It.Is<string>(s => s == "ExternalSource"))).Returns(mockConfSection.Object);

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            HttpResponseMessage result = new HttpResponseMessage();
            result.StatusCode = System.Net.HttpStatusCode.OK;
            result.Content = new StringContent($"[[1672531200000,16569,{expectedCloseValue},16569,16541,8.82293957]]");

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

            mockHttpClientFactory.Setup(_ => _.CreateClient("bitfinex")).Returns(httpClient);

            var service = new Bitfinex(mockHttpClientFactory.Object, mockConfiguration.Object);

            // Act
            var resultPrice = await service.GetPriceByHourAsync(CurrencyCode.BTC, CurrencyCode.USD, expectedStartDateTime, expectedEndDateTime);

            // Assert
            Assert.Equal(expectedToCurrency, resultPrice?.ToCurrency.ToString());
            Assert.Equal(expectedFromCurrency, resultPrice?.FromCurrency.ToString());
            Assert.Equal(expectedCloseValue, resultPrice?.Close.ToString());
            Assert.Equal(expectedStartDateTime, resultPrice?.StartDateTime);

            handlerMock.VerifyCalls(r => r.RequestUri.Query
            .Contains($"?start={expectedStartDateTime.ConvertFromDateToUnixTimeMillisecondsString()}&end={expectedEndDateTime.ConvertFromDateToUnixTimeMillisecondsString()}&limit=1"));
        }
    }
}