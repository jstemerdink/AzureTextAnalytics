﻿//  
namespace AzureTextAnalytics.Tests.TextAnalyticsService
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;

    using AzureTextAnalytics.Domain;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GetKeyPhrasesShould
    {
        private const string Input = "some crazy text +0- with stuff &*";

        private const string Error = "This is an error";

        [TestMethod]
        public async Task ReturnBadRequestIfEmptyInput()
        {
            var expected = KeyPhraseResult.Build(HttpStatusCode.BadRequest, Constants.KeyPhraseNullInputErrorText);
            var requestor = TextAnalyticsTestHelper.BuildMockRequestor(s => { }, TextAnalyticsTestHelper.GetErrorMessage(Error));
            var sut = TextAnalyticsTestHelper.BuildSut(requestor.Object);

            var result = await sut.GetKeyPhrases(null);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task UrlEncodeInput()
        {
            var expected = $"{Constants.KeyPhraseRequest}{HttpUtility.UrlEncode(Input)}";
            var request = "";

            var requestor = TextAnalyticsTestHelper.BuildMockRequestor(s => request = s, GetMessage());
            var sut = TextAnalyticsTestHelper.BuildSut(requestor.Object);
            await sut.GetKeyPhrases(Input);

            Assert.AreEqual(expected, request);
        }

        [TestMethod]
        public async Task DecodeResponse()
        {
            var expected = KeyPhraseResult.Build(new[] { "wonderful hotel", "unique decor", "friendly staff" });
            var requestor = TextAnalyticsTestHelper.BuildMockRequestor(s => { }, GetMessage());

            var sut = TextAnalyticsTestHelper.BuildSut(requestor.Object);
            var result = await sut.GetKeyPhrases(Input);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public async Task ReturnFailureOnBadResult()
        {
            var expected = KeyPhraseResult.Build(HttpStatusCode.BadRequest, Error);
            var requestor = TextAnalyticsTestHelper.BuildMockRequestor(s => { }, TextAnalyticsTestHelper.GetErrorMessage(Error));

            var sut = TextAnalyticsTestHelper.BuildSut(requestor.Object);
            var result = await sut.GetKeyPhrases(Input);
            Assert.AreEqual(expected, result);
        }

        private static HttpResponseMessage GetMessage()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content =
                               new StringContent(
                               @"{""odata.metadata"":""https://api.datamarket.azure.com/data.ashx/amla/text-analytics/v1/$metadata"",""KeyPhrases"":[""wonderful hotel"", ""unique decor"", ""friendly staff""]}")
            };
        }
    }
}