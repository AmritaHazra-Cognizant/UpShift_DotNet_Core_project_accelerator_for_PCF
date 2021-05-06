using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WFA.ECS.Framework.Core.Framework.Interception.Castle.Core.AsyncInterceptor
{
	/// <summary>
	/// Intercepts method invocations and determines if is an asynchronous method.
	/// </summary>
	public class AsyncDeterminationInterceptor : IInterceptor
	{
		private static readonly MethodInfo HandleAsyncMethodInfo =
			typeof(AsyncDeterminationInterceptor)
					.GetMethod(nameof(HandleAsyncWithResult), BindingFlags.Static | BindingFlags.NonPublic);

		private static readonly ConcurrentDictionary<Type, GenericAsyncHandler> GenericAsyncHandlers =
			new ConcurrentDictionary<Type, GenericAsyncHandler>();

		/// <summary>
		/// Initializes a new instance of the <see cref="AsyncDeterminationInterceptor"/> class.
		/// </summary>
		public AsyncDeterminationInterceptor(IAsyncInterceptor asyncInterceptor)
		{
			AsyncInterceptor = asyncInterceptor;
		}

		private delegate void GenericAsyncHandler(IInvocation invocation, IAsyncInterceptor asyncInterceptor);

		private enum MethodTYpe
		{
			Synchronous,
			AsyncAction,
			AsyncFunction
		}

		/// <summary>
		/// Gets the underlying async interceptor.
		/// </summary>
		public IAsyncInterceptor AsyncInterceptor { get; }

	/// <summary>
	/// Inetrcepts a method <paramref name="invocation"/>.
	/// </summary>
	/// <param name="invocation">The Method invocation. </param>
	[DebuggerStepThrough]
		public virtual void Intercept(IInvocation invocation)
		{
			MethodType methodType = GetMethodType(invocation.Method.ReturnType);

			switch (methodType)
			{
				case MethodType.AsyncAction:
					AsyncInterceptor.InterceptAsynchronous(invocation);
					return;
				case MethodType.AsyncFunction:
					GetHandler(invocation.Method.ReturnType).Invoke(invocation, AsyncInterceptor);
					return;
				default:
					AsyncInterceptor.InterceptSynchronous(invocation);
					return;
			}
		}

		/// <summary>
		/// Gets the <see cref"MthodType"/> based upon the <paramref name="returnType"/> of the method invocation.
		/// </summary>
		private static MethodType GetMethodType(Type returnType)
		{
			// if there's no return type , or it's not a task, then assume it's a synchronous method.
			if (returnType == typeof(void) || !typeof(Task)) IsAssignableFrom(returnType))
			return MethodTYpe.Synchronous;

			// The return type is a task of some sort, so assume it's asynchronous
			return returnType.GetTypeInfo().IsGenericType ? MethodType.AsyncFunction : MethodType.AsyncAction;
		}

		/// <summary>
		/// Gets the <see cref="GenericAsyncHandler"/> for the method invocation <parameter name="returnType"/>.
		/// </summary>
		private static GenericAsyncHandler GetHandler(Type returnType)
		{
			GenericAsyncHandler handler = GenericAsyncHandlers.GetOrAdd(returnType, CreateHandler);
			return handler;
		}

		/// <summary>
		/// Creates the generic delegate for the <parameter name="returnType"/> method invocation.
		/// </summary>
		private static GenericAsyncHandler CreateHandler(Type returnType)
		{
			Type taskReturnType = returnType.GetGenericArguments()[0];
			MethodInfo method = HandleAsyncMethodInfo.MakeGenericMethod(taskReturnType);
			return (GenericAsyncHandler)method.CreateDelegate(typeof(GenericAsyncHandler));
		}

		/// <summary>
		/// this method is created as a delegate and used to make the call to the generic
		/// <see cref="iasyncinterceptor.interceptasynchronous(t)"/> method.
		/// </summary>
		/// <typeparam name="tresult">the type of the <see cref="task{t}"/> <see cref="task{t}".result/> of the method
		/// <paramref name="invocation"/>.</typeparam> 

		private static void HandleAsyncWithResult<TResult>(IInvocation invocation, IAsyncInterceptor asyncInterceptor)
		{
			asyncInterceptor.InterceptAsynchronous<TResult>(invocation);
		}

	}
}
