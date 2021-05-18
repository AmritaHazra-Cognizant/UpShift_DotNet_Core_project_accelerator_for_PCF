using System;

namespace DotNetCore.Framework.WebServices.Configuration
{
	public interface IServiceBindings
	{
		string EndpointAdress { get; set; }
		BindingConfiguration BindingConfiguration { get; set; }
	}
}