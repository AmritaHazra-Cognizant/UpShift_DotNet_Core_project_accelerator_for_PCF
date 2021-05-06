using System.Threading.Tasks;

namespace WFA.ECS.Framework.Core.Framework.Interception.Castle.Core.AsyncInterceptor
{
	/// <Summary>
	/// Implement this interface to intercept method invocations with DynamicProxy2.
	/// </Summary>
	public interface IAsyncInterceptor
	{
		/// <Summary>
		/// Intercepts a synchronous method <paramref name="invocation"/>.
		/// </Summary>
		/// <param name="invocation">The method invocation. </param>
		void InterceptSynchronous(IInvocation invocation);

		/// <Summary>
		/// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task"/>.
		/// </Summary>
		/// <param name="invocation">The method invocation. </param>
		void InterceptAsynchronous(IInvocation invocation);

		/// <Summary>
		/// Intercepts an asynchronous method <paramref name="invocation"/> with return type of <see cref="Task{T}"/>.
		/// </Summary>
		/// <typeparam name="TResult">The type of the </typeparam>
		/// <param name="invocation">The method invocation. <see cref="Task{T}"/> <see cref="Task{T}.Result"/>.</typeparam>
		void InterceptAsynchronous<TResult>(IInvocation invocation);
	}
}
