using System;
using System.Collections.Generic;
using System.Linq;

namespace WFA.ECS.Framework.Core.Framework.Interception.Castle.Core.AsyncInterceptor
{
	/// <Summary>
	/// Extension methods to <see cref="IProxyGenerator"/> 
	/// </Summary>
	public static class ProxyGeneratorExtensions
	{
		/// <Summary>
		/// Creates an <see cref="IInterceptor"/> for the supplied <paramref name="interceptor"/>.
		/// </Summary>
		/// <param name="interceptor">The interceptor for asynchronous operations. </param>
		/// <returns>The <see cref="IInterceptor"/> for the supplied <paramref name="interceptor"/>.</returns>
		public static IInterceptor ToInteceptor(this IAsyncInterceptor interceptor)
		{
			return new AsyncDeterminationInterceptor(interceptor);
		}

		/// <Summary>
		/// Creates an array of<see cref="IInterceptor"/>objects for the supplied <paramref name="interceptor"/>.
		/// </Summary>
		/// <param name="interceptor">The interceptor for asynchronous operations. </param>
		/// <returns>The <see cref="IInterceptor"/>array for the supplied <paramref name="interceptor"/>.</returns>
		public static IInterceptor[] ToInteceptor(this IEnumerable<IAsyncInterceptor> interceptor)
		{
			return interceptors.Select(ToInteceptor).ToArray();
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateInterfaceProxyWithTarget{TInterface}{TInterface, IInterceptor[]}"/> documentation.
		/// </Summary>
		public static TInterface CreateInterfaceProxyWithTargetAsync<TInterface>(this IProxyGenerator proxyGenerator,
		TInterface target, params IAsyncInterceptor[] interceptors) where TInterface : class
		{
			return proxyGenerator.CreateInterfaceProxyWithTarget(target, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateInterfaceProxyWithTarget{TInterface}{TInterface, IInterceptor[]}"/> documentation.
		/// </Summary>
		public static TInterface CreateInterfaceProxyWithTargetAsync<TInterface>(this IProxyGenerator proxyGenerator,
		TInterface target, ProxyGenerationOptions options, params IAsyncInterceptor[] interceptors) where TInterface : class
		{
			return proxyGenerator.CreateInterfaceProxyWithTarget(target, options, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateInterfaceProxyWithTarget(Type, object , IInterceptor[])"/> documentation.
		/// </Summary>
		public static object CreateInterfaceProxyWithTargetAsync(this IProxyGenerator proxyGenerator,
		Type interfaceToProxy, object target, params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateInterfaceProxyWithTarget(interfaceToProxy, target, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateInterfaceProxyWithTarget(Type, object, ProxyGenerationOptions, IInterceptor[])"/> documentation.
		/// </Summary>
		public static object CreateInterfaceProxyWithTargetAsync(this IProxyGenerator proxyGenerator,
		Type interfaceToProxy, object target, ProxyGenerationOptions options, params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateInterfaceProxyWithTarget(interfaceToProxy, target, options, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateInterfaceProxyWithTarget(Type, Type[], object, IInterceptor[])"/> documentation.
		/// </Summary>
		public static object CreateInterfaceProxyWithTargetAsync(this IProxyGenerator proxyGenerator,
		Type interfaceToProxy, Type[] additionalInterfacesToProxy, object target,
		params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateInterfaceProxyWithTarget(interfaceToProxy, additionalInterfacesToProxy, target, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateInterfaceProxyWithTarget(Type, Type[], object, ProxyGenerationOptions, IInterceptor[])"/> documentation.
		/// </Summary>
		public static object CreateInterfaceProxyWithTargetAsync(this IProxyGenerator proxyGenerator,
		Type interfaceToProxy, Type[] additionalInterfacesToProxy, object target,
		ProxyGenerationOptions options, params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateInterfaceProxyWithTarget(interfaceToProxy, additionalInterfacesToProxy, target, options, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateInterfaceProxyWithTargetInterface(Type, object, IInterceptor[])"/> documentation.
		/// </Summary>
		public static object CreateInterfaceProxyWithTargetInterfaceAsync(this IProxyGenerator proxyGenerator,
		Type interfaceToProxy, object target,
		params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateInterfaceProxyWithTargetInterface(interfaceToProxy, target, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateInterfaceProxyWithTargetInterface{TInterface}{TInterface, IInterceptor[]}"/> documentation.
		/// </Summary>
		public static object CreateInterfaceProxyWithTargetInterfaceAsync<TInterface>(this IProxyGenerator proxyGenerator,
		TInterface target,
		params IAsyncInterceptor[] interceptors) where TInterface : class
		{
			return proxyGenerator.CreateInterfaceProxyWithTargetInterface(target, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateInterfaceProxyWithTargetInterface{TInterface}{TInterface, ProxyGenerationOptions,IInterceptor[]}"/> documentation.
		/// </Summary>
		public static object CreateInterfaceProxyWithTargetInterfaceAsync<TInterface>(this IProxyGenerator proxyGenerator,
		TInterface target, ProxyGenerationOptions options
		params IAsyncInterceptor[] interceptors) where TInterface : class
		{
			return proxyGenerator.CreateInterfaceProxyWithTargetInterface(target, options, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateInterfaceProxyWithTargetInterface(Type, Type[], object, ProxyGenerationOptions, IInterceptor[])"/> documentation.
		/// </Summary>
		public static object CreateInterfaceProxyWithTargetInterfaceAsync(this IProxyGenerator proxyGenerator,
		Type interfaceToProxy, Type[] additionalInterfacesToProxy, object target,
		 params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateInterfaceProxyWithTargetInterface(interfaceToProxy, additionalInterfacesToProxy, target, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateInterfaceProxyWithTargetInterface(Type, Type[], object, IInterceptor[])"/> documentation.
		/// </Summary>
		public static object CreateInterfaceProxyWithTargetInterfaceAsync(this IProxyGenerator proxyGenerator,
		Type interfaceToProxy, object target, ProxyGenerationOptions options
		params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateInterfaceProxyWithTargetInterface(interfaceToProxy, target, options, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateInterfaceProxyWithTargetInterface(Type, Type[], object, ProxyGenerationOptions, IInterceptor[])"/> documentation.
		/// </Summary>
		public static object CreateInterfaceProxyWithTargetInterfaceAsync(this IProxyGenerator proxyGenerator,
		Type interfaceToProxy, Type[] additionalInterfacesToProxy, object target,
		ProxyGenerationOptions options, params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateInterfaceProxyWithTargetInterface(interfaceToProxy, additionalInterfacesToProxy, target, options, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxyWithTarget{TClass}(TClass, IInterceptor[])"/> documentation.
		/// </Summary>
		public static TClass CreateClassProxyWithTargetAsync<TClass>(this IProxyGenerator proxyGenerator,
		TClass target,
		params IAsyncInterceptor[] interceptors) where TClass : class
		{
			return proxyGenerator.CreateClassProxyWithTarget(target, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxyWithTarget{TClass}(TClass, ProxyGenerationOptions, IInterceptor[])"/> documentation.
		/// </Summary>
		public static TClass CreateClassProxyWithTargetAsync<TClass>(this IProxyGenerator proxyGenerator,
		TClass target, ProxyGenerationOptions options
		params IAsyncInterceptor[] interceptors) where TClass : class
		{
			return proxyGenerator.CreateClassProxyWithTarget(target, options, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxyWithTarget(Type, Type[], object, IInterceptor[])"/> documentation.
		/// </Summary>
		public static object CreateClassProxyWithTargetAsync(this IProxyGenerator proxyGenerator,
		Type classToProxy, Type[] additionalInterfacesToProxy, object target,
		params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateClassProxyWithTarget(classToProxy, additionalInterfacesToProxy, target, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxyWithTarget(Type, object, ProxyGenerationOptions, object[], IInterceptor[])"/> documentation.
		/// </Summary>
		public static object CreateClassProxyWithTargetAsync(this IProxyGenerator proxyGenerator,
		Type classToProxy, object target, ProxyGenerationOptions options,
		object[] constructorArguments, params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateClassProxyWithTarget(classToProxy, target, options, constructorArguments, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxyWithTarget(Type, object, object[], IInterceptor[])"/> documentation.
		/// </Summary>
		public static object CreateClassProxyWithTargetAsync(this IProxyGenerator proxyGenerator,
		Type classToProxy, object target,
		object[] constructorArguments, params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateClassProxyWithTarget(classToProxy, target, constructorArguments, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxyWithTarget(Type, object, IInterceptor[])"/> documentation.
		/// </Summary>
		public static object CreateClassProxyWithTargetAsync(this IProxyGenerator proxyGenerator,
		Type classToProxy, object target,
		params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateClassProxyWithTarget(classToProxy, target, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxyWithTarget(Type, object, ProxyGenerationOptions, IInterceptor[])"/> documentation.
		/// </Summary>
		public static object CreateClassProxyWithTargetAsync(this IProxyGenerator proxyGenerator,
		Type classToProxy, object target,
		ProxyGenerationOptions options, params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateClassProxyWithTarget(classToProxy, target, options, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxyWithTarget(Type, Type[], object, ProxyGenerationOptions, IInterceptor[])"/> documentation.
		/// </Summary>
		public static object CreateClassProxyWithTargetAsync(this IProxyGenerator proxyGenerator,
		Type classToProxy, Type[] additionalInterfacesToProxy, object target,
		ProxyGenerationOptions options, params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateClassProxyWithTarget(classToProxy, additionalInterfacesToProxy, target, options, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxyWithTarget(Type, Type[], object, ProxyGenerationOptions, object[],IInterceptor[])"/> documentation.
		/// </Summary>
		public static object CreateClassProxyWithTargetAsync(this IProxyGenerator proxyGenerator,
		Type classToProxy, Type[] additionalInterfacesToProxy, object target,
		ProxyGenerationOptions options, object[] constructorArguments, params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateClassProxyWithTarget(classToProxy, additionalInterfacesToProxy, target, options, constructorArguments, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxy{TClass}(IInterceptor[])"/> documentation.
		/// </Summary>
		public static TClass CreateClassProxyAsync<TClass>(this IProxyGenerator proxyGenerator,
		params IAsyncInterceptor[] interceptors) where TClass : class
		{
			return proxyGenerator.CreateClassProxy<TClass>(interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxy{TClass}(ProxyGenerationOptions, IInterceptor[])"/> documentation.
		/// </Summary>
		public static TClass CreateClassProxyAsync<TClass>(this IProxyGenerator proxyGenerator,
		ProxyGenerationOptions options
		params IAsyncInterceptor[] interceptors) where TClass : class
		{
			return proxyGenerator.CreateClassProxy<TClass>(options, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxy(Type, Type[], IInterceptor[])"/> documentation.
		/// </Summary>
		public static TClass CreateClassProxyAsync(this IProxyGenerator proxyGenerator,
		Type classToProxy, Type[] additionalInterfacesToProxy,
		params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateClassProxy(classToProxy, additionalInterfacesToProxy, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxy(Type, ProxyGenerationOptions, object[], IInterceptor[])"/> documentation.
		/// </Summary>
		public static TClass CreateClassProxyAsync(this IProxyGenerator proxyGenerator,
		Type classToProxy, ProxyGenerationOptions options,
		object[] constructorArguments, params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateClassProxy(classToProxy, options, constructorArguments, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxy(Type, object[], IInterceptor[])"/> documentation.
		/// </Summary>
		public static TClass CreateClassProxyAsync(this IProxyGenerator proxyGenerator,
		Type classToProxy,
		object[] constructorArguments, params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateClassProxy(classToProxy, constructorArguments, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxy(Type, IInterceptor[])"/> documentation.
		/// </Summary>
		public static TClass CreateClassProxyAsync(this IProxyGenerator proxyGenerator,
		Type classToProxy,
		params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateClassProxy(classToProxy, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxy(Type, ProxyGenerationOptions, IInterceptor[])"/> documentation.
		/// </Summary>
		public static TClass CreateClassProxyAsync(this IProxyGenerator proxyGenerator,
		Type classToProxy, ProxyGenerationOptions options,
		params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateClassProxy(classToProxy, options, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxy(Type, Type[], ProxyGenerationOptions, IInterceptor[])"/> documentation.
		/// </Summary>
		public static TClass CreateClassProxyAsync(this IProxyGenerator proxyGenerator,
		Type classToProxy, Type[] additionalInterfacesToProxy, ProxyGenerationOptions options,
		params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateClassProxy(classToProxy, additionalInterfacesToProxy, options, interceptors.ToInteceptors());
		}

		/// <Summary>
		/// See the<see cref="IProxyGenerator.CreateClassProxy(Type, Type[], ProxyGenerationOptions,object[], IInterceptor[])"/> documentation.
		/// </Summary>
		public static TClass CreateClassProxyAsync(this IProxyGenerator proxyGenerator,
		Type classToProxy, Type[] additionalInterfacesToProxy, ProxyGenerationOptions options,
		object[] constructorArguments, params IAsyncInterceptor[] interceptors)
		{
			return proxyGenerator.CreateClassProxy(classToProxy, additionalInterfacesToProxy, options, constructorArguments, interceptors.ToInteceptors());
		}

	}
}
