using FroniusSolarClient.Entities.SolarAPI.V1;
using FroniusSolarClient.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FroniusSolarClient
{
    /// <summary>
    /// Handles HTTP request/response to the Fronius Solar API
    /// </summary>
    internal class RestClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;
        private readonly ILogger _logger;

        public RestClient(HttpClient httpClient, string url, ILogger logger)
        {
            if (String.IsNullOrEmpty(url))
                throw new ArgumentException("URL not specified");

            this._httpClient = httpClient ?? new HttpClient();
            this._url = url;

            _logger = logger;
        }

        /// <summary>
        /// Prepares the HTTP request
        /// </summary>
        /// <returns></returns>
        public HttpRequestMessage PrepareHTTPMessage(string cgi)
        {
            var requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri($"{_url}{cgi}");
            requestMessage.Method = HttpMethod.Get;
            _logger.LogInformation($"RequestUri: {requestMessage.RequestUri}");
            return requestMessage;
        }

        public Task<HttpResponseMessage> ExecuteRequestAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
        {
            return _httpClient.SendAsync(requestMessage, cancellationToken);
        }

        public async Task<Response<T>> GetResponseAsync<T>(string endpoint, CancellationToken cancellationToken)
        {
            try
            {

                var httpRequest = PrepareHTTPMessage(endpoint);

                var httpResponse = await ExecuteRequestAsync(httpRequest, cancellationToken).ConfigureAwait(false);

                httpResponse.EnsureSuccessStatusCode();

                var content = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                _logger.LogInformation($"Response Code: {httpResponse.StatusCode.ToString()}");
                _logger.LogDebug($"Content: {content}");

                var response = JsonHelper.DeSerializeResponse<Response<T>>(content);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured: {ex.Message}");
                return null;
            }
        }
    }
}
