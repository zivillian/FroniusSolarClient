using FroniusSolarClient.Entities.SolarAPI.V1;
using FroniusSolarClient.Entities.SolarAPI.V1.ArchiveData;
using FroniusSolarClient.Entities.SolarAPI.V1.InverterRealtimeData;
using FroniusSolarClient.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using FroniusSolarClient.Entities.SolarAPI.V1.PowerFlowRealtimeData;

namespace FroniusSolarClient
{
    /// <summary>
    /// Obtain data from various Fronius devices (inverters, SensorCards, StringControls)
    /// </summary>
    public class SolarClient
    {
        private readonly RestClient _restClient;
        private readonly SolarClientConfiguration _configuration;

        // Services
        private InverterRealtimeDataService _inverterRealtimeDataService;
        private InverterArchiveDataService _inverterArchiveDataService;
        private PowerFlowRealtimeDataService _powerFlowRealtimeDataService;

        public SolarClient(string url, int version, ILogger logger)
        {
            _configuration = new SolarClientConfiguration(url, version);
            _restClient = new RestClient(null, _configuration.GetBaseURL(), logger);
            

            _inverterRealtimeDataService = new InverterRealtimeDataService(_restClient);
            _inverterArchiveDataService = new InverterArchiveDataService(_restClient);
            _powerFlowRealtimeDataService = new PowerFlowRealtimeDataService(_restClient);
        }

        /// <summary>
        /// Get values which are cumulated to generate a system overview. 
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public Task<Response<CumulationInverterData>> GetCumulationInverterDataAsync(int deviceId = 1, Scope scope = Scope.Device, CancellationToken cancellationToken = default)
        {
            return _inverterRealtimeDataService.GetCumulationInverterDataAsync(deviceId, scope, cancellationToken);
        }

        /// <summary>
        /// Get values which are provided by all types of Fronius inverters. 
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public Task<Response<CommonInverterData>> GetCommonInverterDataAsync(int deviceId = 1, Scope scope = Scope.Device, CancellationToken cancellationToken = default)
        {
            return _inverterRealtimeDataService.GetCommonInverterDataAsync(deviceId, scope, cancellationToken);
        }

        /// <summary>
        /// Get values which are provided by 3phase Fronius inverters. 
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public Task<Response<P3InverterData>> GetP3InverterDataAsync(int deviceId = 1, Scope scope = Scope.Device, CancellationToken cancellationToken = default)
        {
            return _inverterRealtimeDataService.GetP3InverterDataAsync(deviceId, scope, cancellationToken);
        }

        /// <summary>
        /// Get minimum and maximum values of various inverter values.  
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public Task<Response<MinMaxInverterData>> GetMinMaxInverterDataAsync(int deviceId = 1, Scope scope = Scope.Device, CancellationToken cancellationToken = default)
        {
            return _inverterRealtimeDataService.GetMinMaxInverterDataAsync(deviceId, scope, cancellationToken);
        }

        /// <summary>
        /// Get archived data whenever access to historic device-data is possible. The number of days stored is dependant on the number of connected units that are logging data.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="channels"></param>
        /// <param name="deviceId"></param>
        /// <param name="scope"></param>
        /// <param name="seriesType"></param>
        /// <param name="humanReadable"></param>
        /// <param name="deviceClass"></param>
        /// <returns></returns>
        public Task<Response<Dictionary<string, ArchiveData>>> GetArchiveDataAsync(DateTime startDate, DateTime endDate, List<Channel> channels, int deviceId = 1, Scope scope = Scope.System, SeriesType seriesType = SeriesType.DailySum, bool humanReadable = true, DeviceClass deviceClass = DeviceClass.Inverter, CancellationToken cancellationToken = default)
        {
            return _inverterArchiveDataService.GetArchiveDataAsync(startDate, endDate, channels, deviceId, scope, seriesType, humanReadable, deviceClass, cancellationToken);
        }

        public Task<Response<PowerFlowRealtimeData>> GetPowerFlowRealtimeDataAsync(CancellationToken cancellationToken = default)
        {
            return _powerFlowRealtimeDataService.GetPowerFlowRealtimeDataAsync(cancellationToken);
        }
    }
}
