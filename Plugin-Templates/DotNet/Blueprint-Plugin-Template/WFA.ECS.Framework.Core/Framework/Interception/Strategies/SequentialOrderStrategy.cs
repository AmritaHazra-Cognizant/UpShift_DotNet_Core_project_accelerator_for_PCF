using System;
using System.Collections.Generic;
using WFA.ECS.Framework.Core.Framework.Interception.Interfaces;

namespace WFA.ECS.Framework.Core.Framework.Interception.Strategies
{
	/// <summary>
	/// Sequential Ordering Strategy for Interceptors
	/// </summary>
	public sealed class SequentialOrderStrategy : IOrderingStrategy
	{
		/// <inheritdoc />
		public IEnumerable<InvocationContext> OrderBeforeInterception(IEnumerable<InvocationContext> interceptors)
		{
			return interceptors;
		}

		/// <inheritdoc />
		public IEnumerable<InvocationContext> OrderAfterInterception(IEnumerable<InvocationContext> interceptors)
		{
			return interceptors;
		}
	}
}
