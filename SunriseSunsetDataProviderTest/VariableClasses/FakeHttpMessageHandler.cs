using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SunriseSunsetDataProviderTest.VariableClasses
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        private HttpResponseMessage Response;

        public FakeHttpMessageHandler()
        {
        }

        protected override void Dispose(bool disposing)
        {
        }

        public void SetHttpMessageMessage(string content, HttpStatusCode httpStatusCode)
        {
            var memStream = new MemoryStream();

            var sw = new StreamWriter(memStream);
            sw.Write(content);
            sw.Flush();
            memStream.Position = 0;

            var httpContent = new StreamContent(memStream);

            var response = new HttpResponseMessage()
            {
                StatusCode = httpStatusCode,
                Content = httpContent
            };

            Response = response;
        }


        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<HttpResponseMessage>();

            tcs.SetResult(Response);

            return tcs.Task;
        }
    }
}
