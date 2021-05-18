namespace DotNetCore.Framework.Interception.Configuration
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;
	using DotNetCore.Framework.Interception.Attributes;
	using DotNetCore.Framework.Interception.Interfaces;
	using DotNetCore.Framework.Interception.Internal.Configuration;
	using DotNetCore.Framework.Interception.Internal.Interfaces;
	using DotNetCore.Framework.Interception.Strategies;

	/// <summary>
	/// Configuration class for Proxy Generation
	/// </summary>
	[ExcludeFromCodeCoverage]
	public sealed class SimpleProxyConfiguration
	{
		/// <summary>
		/// Gets or sets a Collection of all Configured Interceptors
		/// </summary>
		internal List<IInterceptorMapping> ConfiguredInterceptors { get; set; } = new List<IInterceptorMapping>();

		/// <summary>
		/// Gets or sets the ordering Strategy for Interceptors
		/// </summary>
		internal IOrderingStrategy OrderingStrategy { get; set; } = new PyramidOrderStrategy();

		/// <summary>
		/// Gets or sets a value which determines whether invalid Interceptors are ignored (rather than throw exceptions)
		/// </summary>
		internal bool IgnoreInvalidInterceptors = true;

		/// <summary>
		/// Adds an Interceptor to the Configuration
		/// </summary>
		/// <typeparam name="TAttribute">Attribute to trigger Interception </typeparam>
		/// <typeparam name="TInterceptor">Attribute to call when Attribute is applied </typeparam>
		/// <returns></returns>
		public SimpleProxyConfiguration AddInterceptor<TAttribute, TInterceptor>() where TAttribute : MethodInterceptionAttribute where TInterceptor : IMethodInterceptor
		{
			// Adds an Interceptor Mapping for matching up Attributes to Interceptors
			this.ConfiguredInterceptors.Add(new InterceptorMapping<TAttribute, TInterceptor>());

			// Return the Proxy Configuration for chaining Configuration
			return this;
		}

		/// <summary>
		/// Prevent exceptions being thrown when inetrceptors are not Configured correctly
		/// </summary>
		/// <returns><see cref="SimpleProxyConfiguration"/> so that Configuration can be chained</returns>
		public SimpleProxyConfiguration IgnoreInvalidInterceptorConfigurations()
		{
			// Invalid Interceptor Configurations are ignored and wont throw exceptions
			this.IgnoreInvalidInterceptors = true;

			// Return the Proxy Configuration for chaining Configuration
			return this;
		}

		/// <summary>
		/// Applies an ordering Strategy to the Interceptors
		/// </summary>
		/// <typeparam name="TStrategy"> Strategy (class) Type </typeparam>
		/// <returns><see cref="SimpleProxyConfiguration"/> so that Configuration can be chained</returns>
		public SimpleProxyConfiguration WithOrderingStrategy<TStrategy>() where TStrategy : IOrderingStrategy
		{
			// Creates a new instance of the ordering Strategy and assigns it the Configuration
			this.OrderingStrategy = Activator.CreateInstance<TStrategy>();

			// Return the Proxy Configuration for chaining Configuration
			return this;
		}

	}
}
