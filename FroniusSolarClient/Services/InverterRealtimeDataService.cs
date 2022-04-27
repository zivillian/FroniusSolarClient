using FroniusSolarClient.Entities.SolarAPI.V1;
using FroniusSolarClient.Entities.SolarAPI.V1.InverterRealtimeData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FroniusSolarClient.Services
{
    /// <summary>
    /// These requests will be provided where direct access to the realtime data of the devices is possible. This is currently the case for the Fronius Datalogger Web and the Fronius Datamanager.
    /// </summary>
    internal class InverterRealtimeDataService : BaseDataService
    {
        private readonly string _cgi = "GetInverterRealtimeData.cgi";

        public InverterRealtimeDataService(RestClient restClient) 
            : base(restClient)
        {
        }


        /// <summary>
        /// Builds the query string for the request
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected string BuildQueryString(int deviceId, Scope scope, DataCollection dataCollection)
        {
            //TODO: Support list of string dictionary to build HTTP query string
            return $"?Scope={scope}&DeviceId={deviceId}&DataCollection={dataCollection}";
        }

        public Task<Response<CumulationInverterData>> GetCumulationInverterDataAsync(int deviceId = 1, Scope scope = Scope.Device, CancellationToken cancellationToken = default)
        {
            string baseEndpointURL = _cgi + BuildQueryString(deviceId, scope, DataCollection.CumulationInverterData);           
            return GetDataServiceResponseAsync<CumulationInverterData>(baseEndpointURL, cancellationToken);
        }

        public  Task<Response<CommonInverterData>> GetCommonInverterDataAsync(int deviceId = 1, Scope scope = Scope.Device, CancellationToken cancellationToken = default)
        {
            string baseEndpointURL = _cgi + BuildQueryString(deviceId, scope, DataCollection.CommonInverterData);
            return GetDataServiceResponseAsync<CommonInverterData>(baseEndpointURL, cancellationToken); 
        }


        public  Task<Response<P3InverterData>> GetP3InverterDataAsync(int deviceId = 1, Scope scope = Scope.Device, CancellationToken cancellationToken = default)
        {
            string param = $"?Scope={scope.ToString()}&DeviceId={deviceId}&DataCollection=3PInverterData";
            string baseEndpointURL = _cgi + param;
            return GetDataServiceResponseAsync<P3InverterData>(baseEndpointURL, cancellationToken);
        }


        public  Task<Response<MinMaxInverterData>> GetMinMaxInverterDataAsync(int deviceId = 1, Scope scope = Scope.Device, CancellationToken cancellationToken = default)
        {
            string baseEndpointURL = _cgi + BuildQueryString(deviceId, scope, DataCollection.MinMaxInverterData);
            return GetDataServiceResponseAsync<MinMaxInverterData>(baseEndpointURL, cancellationToken);
        }

    }
}
