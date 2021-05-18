using DotNetCore.API.DataService.Contracts;
using DotNetCore.Framework.WebServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNetCore.API.Logging;
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
        public SampleSac(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _client = serviceProvider.GetRequiredService<IEndpointFactory<IWcfService>>();
        }
        [LogMethod("SAC001","Error occured during RetrieveHellowWorld call.")]
        public async Task<string> RetrieveHellowWorld()
        {
            var result = await CreateProxy().HelloWorldSoapAsync();
            return result;
        }

        private IWcfService CreateProxy()
        {
            var proxy = _client.CreateChannel();
            return proxy;
        }
    }
}
