using System;

namespace WFA.ECS.Framework.Core.Framework.Interception.Internal.Interfaces
{
	/// <summary>
	/// Interceptor Mapping Interface 
	/// </sumary>
	internal interface IInterceptorMapping
	{
		/// <summary>
		/// Attribute Type 
		/// </sumary>
		Type AttributeType { get; }

		/// <summary>
		/// Interceptor Type 
		/// </sumary>
		Type InterceptorType { get; }
	}
}
