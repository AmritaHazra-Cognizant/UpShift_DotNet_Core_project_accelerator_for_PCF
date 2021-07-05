using System;

namespace DotNetCore.Framework.WebServices.Configuration
{
	public interface IBindingConfiguration
	{
		string CloseTimeout { get; set; }
		string OpenTimeout { get; set; }
		string ReceiveTimeout { get; set; }
		string SendTimeout { get; set; }
		string AllowCookies { get; set; }
		string BypassProxyOnLocal { get; set; }
		string MaxBufferSize { get; set; }
		string MaxBufferPoolSize { get; set; }
		string MaxReceivedMessageSize { get; set; }
		string UseDefaultWebProxy { get; set; }
		ReaderQuotas ReaderQuotas { get; set; }
	}

	public class BindingConfiguration : IBindingConfiguration
	{
		public string CloseTimeout { get; set; }
		public string OpenTimeout { get; set; }
		public string ReceiveTimeout { get; set; }
		public string SendTimeout { get; set; }
		public string AllowCookies { get; set; }
		public string BypassProxyOnLocal { get; set; }
		public string MaxBufferSize { get; set; }
		public string MaxBufferPoolSize { get; set; }
		public string MaxReceivedMessageSize { get; set; }
		public string UseDefaultWebProxy { get; set; }
		public ReaderQuotas ReaderQuotas { get; set; }
	}
}