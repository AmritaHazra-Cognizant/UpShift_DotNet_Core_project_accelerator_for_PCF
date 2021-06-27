using System;

namespace DotNetCore.Framework.WebServices
{
	/// <summary>
	/// Defines required operations for executing synchronous Func-delegates using a proxy wcf client.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IEndpointFactory<out T>
	{
		T CreateChannel();
	}
}