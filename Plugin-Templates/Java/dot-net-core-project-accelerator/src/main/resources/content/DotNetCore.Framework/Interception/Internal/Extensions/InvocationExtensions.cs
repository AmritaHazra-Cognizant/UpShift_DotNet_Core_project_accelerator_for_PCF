namespace DotNetCore.Framework.Interception.Internal.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Attributes;
	using Castle.DynamicProxy;
	using Exceptions;
	using Microsoft.Extensions.DependencyInjection;
	using DotNetCore.Framework.Interception.Configuration;
	using DotNetCore.Framework.Interception.Interfaces;
	using DotNetCore.Framework.Interception;

	/// <summary>
	/// Extension methods for <see cref="IInvocation"/>
	/// </summary>
	internal static class InvocationExtensions
	{
		/// <summary>
		/// Gets a colection of all the interceptors associated with the current executing method
		/// </summary>
		/// <param name="invocation">current invocation</param>
		/// <param name="serviceProvider">service provider instance</param>
		/// <param name="proxyConfiguration">Proxy Configuration</param>
		/// <returns><see cref="Dictionary{TKey,TValue}"/> of all configured interceptors for this method</returns>
		internal static List<InvocationContext> GetInterceptorMetadataForMethod(IInvocation invocation, IServiceProvider serviceProvider, SimpleProxyConfiguration proxyConfiguration)
		{
			//Creates the interceptor list to store the configured interceptors
			var interceptorList = new List<InvocationContext>();

			//Get the Attributes applied to the method being invoked
			var methodAttributes = invocation
			.MethodInvocationTarget
			.GetCustomAttributes()
			.Where(p => p.GetType().IsSubclassOf(typeof(MethodInterceptionAttribute)))
			.Cast<MethodInterceptionAttribute>();

			var index = 0;
			foreach(var methodAttribute in methodAttributes)
			{
				// Get the interceptor that is bound to the attribute
				var interceptorType = proxyConfiguration.ConfiguredInterceptors.FirstOrDefault(p => p.AttributeType == methodAttribute.GetType())?.InterceptorType;
				if(interceptorType == null)
				{
					if(proxyConfiguration.IgnoreInvalidInterceptors)
					{
						continue;
					}
					throw new InvalidInterceptorException($"The Interceptor Attribute '{methodAttribute}' is applied to the method, but there is no configured binterceptor to handle it");
				}

				// Use the serviceProvider to Create the Interceptor instance so you can inject dependencies into the constructor
				var instance = (IMethodInterceptor)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, interceptorType);

				//New InvocationContext instance
				var context = new InvocationContext
				{
					Attribute = methodAttribute,
					Interceptor = instance,
					Invocation = invocation,
					Order = index,
					ServiceProvider = serviceProvider
				};

				interceptorList.Add(context);
				index += 1;
			}

			//Returns the list of configured interceptors 
			return interceptorList;
		}

	}
	
}