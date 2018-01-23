using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DealCloud.AddIn.Common.Utils;
using DealCloud.Common;
using DealCloud.Common.Entities;
using DealCloud.Common.Enums;
using DealCloud.Common.Serialization;
using NLog;

namespace DealCloud.AddIn.Common
{
    public class HttpClientUtil : IDisposable
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public CookieContainer CookieContainer { get; set; }

        public string AddinVersion { get; set; }

        public AddinTypes AddinType { get; set; }

        public TimeSpan Timeout { get; set; }

        public Uri BaseAddress { get; set; }

        private string AntiForgeryToken { get; set; }

        public HttpClientUtil()
        {
#if DEBUG
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => true;
#endif
        }

        protected HttpClient CreateHttpClient()
        {
            HttpClient client = new HttpClient(new HttpClientHandler
            {
                CookieContainer = CookieContainer,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            })
            {
                Timeout = Timeout,
                BaseAddress = BaseAddress
            };
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            client.DefaultRequestHeaders.Add(Constants.AFT_REQUEST_HEADER_NAME, AntiForgeryToken);
            client.DefaultRequestHeaders.Add(Constants.ADDIN_VERSION_INFO, AddinVersion);
            return client;
        }

        public void Dispose()
        {

        }

        public class Parameter
        {
            public string Name { get; set; }
            public object Value { get; set; }
            public bool Serialize { get; set; }
        }

        public async Task<T> PostJsonAsync<T>(string methdoUrl, object param = null, CancellationToken? cancellationToken = null)
        {
            using (HttpClient client = CreateHttpClient())
            {
                var content = new StringContent(param != null ? SerializationHelper.SerializeNewtonsoftJson(param, true) : null, Encoding.UTF8, "application/json");
                var response = await PostAsyn(methdoUrl, client, content, cancellationToken ?? CancellationToken.None).ConfigureAwait(false);
                var responseData = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                return SerializationHelper.DeserializeNewtonsoftJson<T>(Encoding.UTF8.GetString(responseData), true);
            }
        }

        public async Task PostJsonAsync(string methdoUrl, object param = null, CancellationToken? cancellationToken = null)
        {
            using (HttpClient client = CreateHttpClient())
            { 
                var content = new StringContent(param != null ? SerializationHelper.SerializeNewtonsoftJson(param, true) : null, Encoding.UTF8, "application/json");
                await PostAsyn(methdoUrl, client, content, cancellationToken ?? CancellationToken.None).ConfigureAwait(false);
            }
        }

        public async Task<T> PostMultipartFormAsync<T>(string methdoUrl, IEnumerable<Parameter> values = null, CancellationToken? cancellationToken = null)
        {
            using (HttpClient client = CreateHttpClient())
            using (MultipartFormDataContent content = new MultipartFormDataContent())
            {
                int paramIndex = 0;
                if (values != null)
                {
                    foreach (var parameter in values)
                    {
                        if (parameter.Value is byte[])
                        {
                            ByteArrayContent contentBytes = new ByteArrayContent(parameter.Value as byte[]);
                            contentBytes.Headers.Add("Content-Type", "application/gzip");
                            content.Add(contentBytes, $"file{paramIndex}", parameter.Name);
                        }
                        else if (parameter.Serialize)
                        {
                            StringContent contentString = new StringContent(SerializationHelper.SerializeNewtonsoftJson(parameter.Value, true), Encoding.UTF8, "application/json");
                            content.Add(contentString, parameter.Name);
                        }
                        else
                        {
                            StringContent contentString = new StringContent(parameter.Value?.ToString(), Encoding.UTF8, "text/plain");
                            content.Add(contentString, parameter.Name);
                        }
                        paramIndex++;
                    }
                }
                var response = await PostAsyn(methdoUrl, client, content, cancellationToken ?? CancellationToken.None).ConfigureAwait(false);
                var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                return SerializationHelper.DeserializeNewtonsoftJson<T>(Encoding.UTF8.GetString(bytes), true);
            }
        }

        public async Task<T> GetJsonAsync<T>(string methdoUrl, Parameter[] parameters = null, CancellationToken? cancellationToken = null)
        {
            using (HttpClient client = CreateHttpClient())
            {
                var response = await GetAsync(methdoUrl, client, parameters, cancellationToken).ConfigureAwait(false);
                
                var responseData = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                return SerializationHelper.DeserializeNewtonsoftJson<T>(Encoding.UTF8.GetString(responseData), true);
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string methdoUrl, Parameter[] parameters = null, CancellationToken? cancellationToken = null)
        {
            using (HttpClient client = CreateHttpClient())
            {
                return await GetAsync(methdoUrl, client, parameters, cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task<HttpResponseMessage> GetAsync(string methdoUrl, HttpClient client, Parameter[] parameters = null, CancellationToken? cancellationToken = null)
        {
            var url = new StringBuilder($"{methdoUrl}");
            var cancellationTokenDeafult = cancellationToken ?? CancellationToken.None;
            if (parameters != null && parameters.Any())
            {
                parameters.Aggregate(url.Append("?"), (builder, parameter) => builder.Append($"{parameter.Name}={Uri.EscapeUriString(parameter.Value?.ToString())}&"));
            }
            if (Log.IsDebugEnabled)
            {
                //Log.Debug($"HTTP GET BEFORE with Url: '{url}'");
                //Log.Debug($"HTTP GET BEFORE with Cookies: \n{CookieContainer.ToString(BaseAddress)}");
            }
            var response = await client.GetAsync(url.ToString(), cancellationTokenDeafult).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                ExtractAntiForgeryToken(response);
                return response;
            }
            var responce = await response.Content.ReadAsStringAsync();
            if (Log.IsDebugEnabled)
            {
                Log.Debug($"HTTP GET FAILED with RequestMessage: \n'{response.RequestMessage}'");
                Log.Debug($"HTTP GET FAILED with Cookies: \n{CookieContainer.ToString(BaseAddress)}");
                Log.Debug($"HTTP GET FAILED with Responce: \n'{responce}'");
            }
            throw new HttpClientException(response.ReasonPhrase, response.StatusCode, GetErrorInfo(responce));
        }

        private async Task<HttpResponseMessage> PostAsyn(string methdoUrl, HttpClient client, HttpContent content, CancellationToken cancellationToken)
        {
            if (Log.IsDebugEnabled)
            {
                //Log.Debug($"HTTP POST BEFORE with Url: '{methdoUrl}'");
                //Log.Debug($"HTTP POST BEFORE with Cookies: \n{CookieContainer.ToString(BaseAddress)}");
            }
            var response = await client.PostAsync(methdoUrl, content, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }
            var responce = await response.Content.ReadAsStringAsync();
            if (Log.IsDebugEnabled)
            {
                Log.Debug($"HTTP POST FAILED with RequestMessage: \n'{response.RequestMessage}'");
                Log.Debug($"HTTP POST FAILED with Cookies: \n{CookieContainer.ToString(BaseAddress)}");
                Log.Debug($"HTTP POST FAILED with Responce: \n'{responce}'");
            }
            throw new HttpClientException(response.ReasonPhrase, response.StatusCode, GetErrorInfo(responce));
        }

        private ErrorInfo GetErrorInfo(string responce)
        {
            try
            {
                return SerializationHelper.DeserializeNewtonsoftJson<ErrorInfo>(responce);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void ExtractAntiForgeryToken(HttpResponseMessage response)
        {
            IEnumerable<string> atf;
            IEnumerable<string> atfc;
            if (response.Headers.TryGetValues(Constants.AFT_RESPONSE_HEADER_NAME, out atf) &&
                response.Headers.TryGetValues(Constants.AFTC_RESPONSE_HEADER_NAME, out atfc))
            {
                string token = $"{atfc.First()}:{atf.First()}";
                AntiForgeryToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(token));
            }
        }
    }

    public class HttpClientException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public string ReasonPhrase { get; set; }

        public ErrorInfo ErrorInfo { get; private set; }

        public HttpClientException(string message, HttpStatusCode statusCode):base(message ?? string.Empty)
        {
            ReasonPhrase = message;
            StatusCode = statusCode;
        }

        public HttpClientException(string message, HttpStatusCode statusCode, ErrorInfo errorInfo) : this(message, statusCode)
        {
            this.ErrorInfo = errorInfo;
        }
    }
}
