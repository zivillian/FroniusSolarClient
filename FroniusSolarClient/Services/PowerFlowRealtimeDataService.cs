using FroniusSolarClient.Entities.SolarAPI.V1;
using FroniusSolarClient.Entities.SolarAPI.V1.PowerFlowRealtimeData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FroniusSolarClient.Services
{
    internal class PowerFlowRealtimeDataService : BaseDataService
    {
        private readonly string _cgi = "GetPowerFlowRealtimeData.fcgi";

        public PowerFlowRealtimeDataService(RestClient restClient)
            : base(restClient)
        {
        }

        /// <summary>
        /// This request provides detailed information about the local energy grid. The values replied represent the current state. 
        /// Because of data has multiple asynchronous origins it is a matter of facts that the sum of all powers (grid,load and generate) will differ from zero. 
        /// This request does not care about the configured visibility of single inverters. All inverters are reported.
        /// </summary>
        /// <returns></returns>
        public Task<Response<PowerFlowRealtimeData>> GetPowerFlowRealtimeDataAsync(CancellationToken cancellationToken)
        {
            return GetDataServiceResponseAsync<PowerFlowRealtimeData>(_cgi, cancellationToken);
        }
    }
}
