using System;
using System.Diagnostics.CodeAnalysis;

namespace WFA.ECS.Framework.Core.Framework.Interception.Attributes
{
	/// <summary>
	/// Method Interception Attribute (inherit from this to create your interception attributes)
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	[ExcludeFromCodeCoverage]
	public abstract class MethodInterceptionAttribute : Attribute
	{
	}
}
