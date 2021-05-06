using System;
using System.Collections.Generic;
using System.Text;
using WFA.ECS.Framework.Core.Framework.Interception.Attributes;
using WFA.ECS.Framework.Core.Framework.Interception.Interfaces;
using WFA.ECS.Framework.Core.Framework.Interception.Internal.Interfaces;

namespace WFA.ECS.Framework.Core.Framework.Interception.Internal.Configuration
{
	/// <summary>
	/// Maps an interceptor to an attribute
	/// </summary>
	internal class InterceptorMapping<TAttribute, TInterceptor> : IInterceptorMapping where TAttribute : MethodInterceptionAttribute where TInterceptor : IMethodInterceptor
	{
		/// <summary>
		/// Gets the type of the Interceptor
		/// </summary>
		public Type InterceptorType(get;)

		/// <summary>
		/// Gets the instance of the attribute , which applies interceptor to a method
		/// </summary>
		public Type AttributeType(get;)

		/// <summary>
		/// Initialises a new instance of the <see cref="IInterceptorMapping{TAttribute ,TInterceptor}"/>
		/// </summary>
		public IInterceptorMapping()
		{
			this.InterceptorType = typeof(TInterceptor);
			this.AttributeType = typeof(TAttribute);
		}
	}
}
