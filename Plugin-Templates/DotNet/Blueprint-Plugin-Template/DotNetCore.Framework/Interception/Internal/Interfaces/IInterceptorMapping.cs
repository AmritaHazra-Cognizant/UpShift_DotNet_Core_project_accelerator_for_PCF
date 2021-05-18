namespace DotNetCore.Framework.Interception.Internal.Interfaces
{
	using System;

	/// <summary>
	/// Interceptor Mapping Interface 
	/// </sumary>
	internal interface IInterceptorMapping
	{
		/// <summary>
		/// Attribute Type 
		/// </sumary>
		Type AttributeType {get; }

		/// <summary>
		/// Interceptor Type 
		/// </sumary>
		Type InterceptorType {get; }
	}
}