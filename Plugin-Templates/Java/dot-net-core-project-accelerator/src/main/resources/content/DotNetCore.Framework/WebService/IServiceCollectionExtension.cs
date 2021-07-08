using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DotNetCore.Framework.WebServices.Configuration;

namespace DotNetCore.Framework.WebServices
{
	public static class IServiceCollectionExtension
	{
		public static  IServiceCollection ConfigureServiceClientProxy(this IServiceCollection services, Action<IServiceBindings> options)
		{
			services.Configure(options);
			return services;
		}
	}
}