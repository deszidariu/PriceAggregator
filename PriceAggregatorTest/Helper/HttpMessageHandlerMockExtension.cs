using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceAggregatorTest.Helper
{
    public static class HttpMessageHandlerMockExtension
    {
        public static void VerifyCalls(this Mock<HttpMessageHandler> mock, Func<HttpRequestMessage, bool> match)
        {
            mock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // we expected a single external request
                ItExpr.Is<HttpRequestMessage>(req => match(req)
                ),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
