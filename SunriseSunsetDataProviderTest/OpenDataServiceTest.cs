using SunriseSunsetDataProvider.Interface;
using SunriseSunsetDataProvider.Model;
using SunriseSunsetDataProviderTest.VariableClasses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace SunriseSunsetDataProviderTest
{
    public class OpenDataServiceTest
    {
        [Theory]
        [InlineData("{}", HttpStatusCode.OK)]
        [InlineData("{\"records\":{}}", HttpStatusCode.OK)]
        [InlineData("{\"records\":{\"locations\":{}}}", HttpStatusCode.OK)]
        [InlineData("", HttpStatusCode.BadRequest)]
        public async void InvalidRespnseData(string response, HttpStatusCode statusCode)
        {
            var fakeHttpMessageHandler = new FakeHttpMessageHandler();
            fakeHttpMessageHandler.SetHttpMessageMessage(response, statusCode);
            var client = new HttpClient(fakeHttpMessageHandler);

            IOpenDataOperater openData = new OpenDataService("http://www.test.url", client);

            var result = await openData.GetData(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), "Test-Token", string.Empty);
            Assert.True(result == null);
        }
    }
}
