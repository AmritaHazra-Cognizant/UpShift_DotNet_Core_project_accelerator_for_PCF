using System;
using System.Collections.Generic;
using System.Text;
using WFA.ECS.Framework.Core.Framework.Interception.Configuration;

namespace WFA.ECS.Framework.Core.Framework.Interception.Internal
{
	/// <summary>
	/// Proxy Factory that generates Proxy Classes
	/// </summary>
	/// <typeparam name="T">Type of object to Proxy </typename>
	public class ProxyFactory<T>
	{
		/// <summary>
		/// Gets or sets the Proxy Configuration
		/// </summary>
		private readonly SimpleProxyConfiguration proxyConfiguration;

		/// <summary>
		/// Gets the CastleCore Proxy Generator
		/// </summary>
		private readonly IProxyGenerator proxyGenerator;

		/// <summary>
		/// Gets the Service Provider
		/// </summary>
		private readonly IServiceProvider serviceProvider;

		/// <summary>
		/// Initialises a new instance of the MasterProxy that wraps an object
		/// </summary>
		/// <param name="serviceProvider">Services Collection</param>
		/// <param name="proxyGenerator">Proxy Generator instance</param>
		/// <param name="config">Proxy Configuration</param>

		public ProxyFactory(IServiceProvider serviceProvider, IProxyGenerator proxyGenerator, SimpleProxyConfiguration config)
		{
			this.proxyConfiguration = config;
			this.proxyGenerator = proxyGenerator;
			this.serviceProvider = serviceProvider;
		}

		///	/// <summary>
		///	/// Creates a proxy object of the object passed in
		///	/// </summary>
		///	/// <param name="originalObject">The Object to be Proxified</param>
		///	/// <returns> The Proxified object </returns>
		///	internal T CreateProxy(T originalObject)
		///	{
		///	// Proxy the original Object
		///	var masterInterceptor = new CoreInterceptor(this.serviceProvider , this.proxyConfiguration);
		///	var proxy = this.proxyGenerator.CreateInterfaceProxyWithTarget(typeof(T), originalObject, masterInterceptor);

		///	// Make sure the proxy was created correctly
		///	if (proxy == null)
		///	{
		///		throw new ArgumentNullException(nameof(proxy));
		///	}

		///	//Return the proxified object
		///	returns (T)proxy;

		///	}

		/// <summary>
		/// Creates a proxy object of the object passed in
		/// </summary>
		/// <param name="originalObject">The Object to be Proxified</param>
		/// <returns> The Proxified object </returns>
		internal T CreateProxy(T originalObject)
		{
			// Proxy the original Object
			var masterInterceptor = new CoreInterceptor(this.serviceProvider, this.proxyConfiguration);
			var proxy = this.proxyGenerator.CreateInterfaceProxyWithTargetInterfaceAsync(typeof(T), originalObject, masterInterceptor);

			// Make sure the proxy was created correctly
			if (proxy == null)
			{
				throw new ArgumentNullException(nameof(proxy));
			}

			//Return the proxified object
			returns(T)proxy;

		}
	}
}
