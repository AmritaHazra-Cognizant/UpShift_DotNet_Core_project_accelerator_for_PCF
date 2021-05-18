using System;

namespace DotNetCore.Framework.WebServices.Configuration
{
	public class ServiceBindings : IServiceBindings
	{
		public const string ServiceBindingsKey = "ServiceBindings";
		public string EndpointAdress{ get; set; }
		public BindingConfiguration BindingConfiguration { get; set; }
	}
}