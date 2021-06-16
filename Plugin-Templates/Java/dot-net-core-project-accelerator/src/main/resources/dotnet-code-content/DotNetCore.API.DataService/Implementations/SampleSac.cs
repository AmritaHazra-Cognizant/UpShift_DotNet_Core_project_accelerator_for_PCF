using DotNetCore.API.DataService.Contracts;
using DotNetCore.Framework.WebServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNetCore.API.Logging;
using DotNetCore.API.Contract;
using DotNetCore.API.DataService.Providers.Api;
using DotNetCore.API.DataService.Providers.Models;
namespace DotNetCore.API.DataService.Implementations
{
    /*
   * This class is used to show sample Service layer example
   * To Do: For Actual project please remove this file 
   * after getting the idea of how to implement.
   */
    public class SampleSac : BaseSac, ISampleSac
    {
        private readonly IEndpointFactory<IWcfService> _client;
        private readonly IEmployeeRestApi _restClient;
        public SampleSac(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _client = serviceProvider.GetRequiredService<IEndpointFactory<IWcfService>>();
            _restClient = serviceProvider.GetRequiredService<IEmployeeRestApi>();
        }

        /// <summary>
        /// Sample SOAP WCF SERVER
        /// </summary>
        /// <returns></returns>
        [LogMethod("SAC001", "Error occured during RetrieveHellowWorld call.")]
        public async Task<string> RetrieveHellowWorld()
        {
            var result = await CreateProxy().HelloWorldSoapAsync();
            return result;
        }

        /// <summary>
        /// Sample REST API Call
        /// </summary>
        /// <returns></returns>
        [LogMethod("REST001", "Error occured during RetrieveEmployees call.")]
        public async Task<List<EmployeeInfo>> RetrieveEmployees()
        {
            var response = await _restClient.ListEmployeeDataAsync();
            if (response != null && response.Data != null && response.Data.Length > 0)
            {
                return MapEmployeeResponse(response.Data);
            }
            else
            {
                return null;
            }

        }
        private List<EmployeeInfo> MapEmployeeResponse(Employee[] data)
        {
            List<EmployeeInfo> employess = new List<EmployeeInfo>();
            for (int index = 0; index < data.Length; index++)
            {
                employess.Add(new EmployeeInfo
                {
                    Avatar = Convert.ToString(data[index].Avatar),
                    Email = data[index].Email,
                    FirstName = data[index].FirstName,
                    Id = data[index].Id,
                    LastName = data[index].LastName

                });
            }

            return employess;

        }
        private IWcfService CreateProxy()
        {
            var proxy = _client.CreateChannel();
            return proxy;
        }
    }
}
