using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace WFA.ECS.Framework.Core.Framework.Interception.Extensions
{
	/// <summary>
	/// Extension methods for <see cref="IServiceCollection"/>
	/// </summary>
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Enable proxy Generation for services registered in  <see cref="IServiceCollection"/>
		/// </summary>
		/// <param name="services">services collection</param>
		/// <param name="Options">Proxy Configuration Options</param>
		/// <returns><see cref="IServiceCollection"/></returns>
		public static IServiceCollection EnableInterceptionProxy(this IServiceCollection services, Action<SimpleProxyConfiguration> options)
		{
			//Check Inputs for Null
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			// Store the Proxy Configuration
			services.Configure(options);

			// Proxy Generator needs to be registered as a Singleton for performance reasons
			services.AddSingleton<IProxyGenerator, ProxyGenerator>();

			// Return the IServiceCollection for chaining Configuration
			return services;
		}


		/// <summary>
		/// Adds a transient service to the  <see cref="ServiceCollection"/> that is wrapped in a Proxy
		/// </summary>
		/// <typeparam name="TInterface">Interface Type</typeparam>
		/// <typeparam name="TService">Implementation Type</typeparam>
		/// <param name="services">services collection</param>
		/// <returns><see cref="IServiceCollection"/></returns>
		public static IServiceCollection AddTransientWithProxy<TInterface, TService>(this IServiceCollection services) where TService : TInterface
		{
			var proxyConfiguration = services.GetProxyConfiguration();

			//Wrap the service with a proxy instance and add it with Scoped Scope
			services.AddTransient(typeof(TInterface), serviceProvider =>
			{
				var proxyGenerator = serviceProvider.GetService<IProxyGenerator>();
				var proxyProvider = new ProxyFactory<TInterface>(serviceProvider, proxyGenerator, proxyConfiguration);
				return proxyProvider.CreateProxy(ActivatorUtilities.CreateInstance<TService>(serviceProvider));
			});

			// Return the IServiceCollection for chaining Configuration
			return services;
		}

		/// <summary>
		/// Adds a Singleton service to the  <see cref="ServiceCollection"/> that is wrapped in a Proxy
		/// </summary>
		/// <typeparam name="TInterface">Interface Type</typeparam>
		/// <typeparam name="TService">Implementation Type</typeparam>
		/// <param name="services">services collection</param>
		/// <returns><see cref="IServiceCollection"/></returns>
		public static IServiceCollection AddSingletontWithProxy<TInterface, TService>(this IServiceCollection services) where TService : TInterface
		{
			var proxyConfiguration = services.GetProxyConfiguration();

			//Wrap the service with a proxy instance and add it with Scoped Scope
			services.AddSingleton(typeof(TInterface), serviceProvider =>
			{
				var proxyGenerator = serviceProvider.GetService<IProxyGenerator>();
				var proxyProvider = new ProxyFactory<TInterface>(serviceProvider, proxyGenerator, proxyConfiguration);
				return proxyProvider.CreateProxy(ActivatorUtilities.CreateInstance<TService>(serviceProvider));
			});

			// Return the IServiceCollection for chaining Configuration
			return services;
		}

		/// <summary>
		/// Gets the Proxy Configuration to pass to the Proxy Factory Method
		/// </summary>
		/// <param name="services"><see cref="IServiceCollection"/></param>
		/// <returns>Proxy Configuration</returns>
		private static SimpleProxyConfiguration GetProxyConfiguration(this IServiceCollection services)
		{
			return services.BuildServiceProvider().GetRequiredService<IOptions<SimpleProxyConfiguration>>().Value;
		}
	}
}
