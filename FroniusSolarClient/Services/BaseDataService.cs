using System.Threading;
using System.Threading.Tasks;
using FroniusSolarClient.Entities.SolarAPI.V1;

namespace FroniusSolarClient.Services
{
    internal class BaseDataService
    {
        private readonly RestClient _restClient;

        public BaseDataService(RestClient restClient)
        {
            _restClient = restClient;
        }

        protected Task<Response<T>> GetDataServiceResponseAsync<T>(string endpointURL, CancellationToken cancellationToken)
        {
            return _restClient.GetResponseAsync<T>(endpointURL, cancellationToken);
        }
    }
}
