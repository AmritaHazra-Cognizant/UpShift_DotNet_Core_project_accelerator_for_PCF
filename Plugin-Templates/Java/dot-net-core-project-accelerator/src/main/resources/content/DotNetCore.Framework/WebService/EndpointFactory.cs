using System;
using System.ServiceModel;
using System.Xml;
using Microsoft.Extensions.Options;
using DotNetCore.Framework.WebServices.Configuration;

namespace DotNetCore.Framework.WebServices
{
	public class EndpointFactory<T> : IEndpointFactory<T> where T : class, ICommunicationObject
	{
		private IServiceBindings _ServiceBindings;
		public EndpointFactory(IOptions<ServiceBindings> serviceBindings)
		{
			_ServiceBindings = serviceBindings.Value;
		
		}

		public virtual T CreateChannel()
		{
			/*binding Configuration*/
			var binding = new BasicHttpBinding();
			var bindingInfo = _ServiceBindings;
			binding.CloseTimeout = TimeSpan.FromSeconds(System.Convert.ToDouble(bindingInfo.BindingConfiguration.CloseTimeout));
			binding.OpenTimeout = TimeSpan.FromSeconds(System.Convert.ToDouble(bindingInfo.BindingConfiguration.OpenTimeout));
			binding.ReceiveTimeout = TimeSpan.FromSeconds(System.Convert.ToDouble(bindingInfo.BindingConfiguration.ReceiveTimeout));
			binding.SendTimeout = TimeSpan.FromSeconds(System.Convert.ToDouble(bindingInfo.BindingConfiguration.SendTimeout));
			binding.AllowCookies = Convert.ToBoolean(bindingInfo.BindingConfiguration.AllowCookies);
			binding.BypassProxyOnLocal = Convert.ToBoolean(bindingInfo.BindingConfiguration.BypassProxyOnLocal);
			binding.MaxBufferSize = Convert.ToInt32(bindingInfo.BindingConfiguration.MaxBufferSize);
			binding.MaxBufferPoolSize = Convert.ToInt64(bindingInfo.BindingConfiguration.MaxBufferPoolSize);
			binding.MaxReceivedMessageSize = Convert.ToInt64(bindingInfo.BindingConfiguration.MaxReceivedMessageSize);
			binding.TextEncoding = System.Text.Encoding.UTF8;
			binding.TransferMode = TransferMode.Buffered;
			binding.UseDefaultWebProxy = Convert.ToBoolean(bindingInfo.BindingConfiguration.UseDefaultWebProxy);
			/*readerQuotas Configuration*/
			var readerQuotas = new XmlDictionaryReaderQuotas();
			readerQuotas.MaxDepth = Convert.ToInt32(bindingInfo.BindingConfiguration.ReaderQuotas.MaxDepth);
			readerQuotas.MaxStringContentLength = Convert.ToInt32(bindingInfo.BindingConfiguration.ReaderQuotas.MaxStringContentLength);
			readerQuotas.MaxArrayLength = Convert.ToInt32(bindingInfo.BindingConfiguration.ReaderQuotas.MaxArrayLength);
			readerQuotas.MaxBytesPerRead = Convert.ToInt32(bindingInfo.BindingConfiguration.ReaderQuotas.MaxBytesPerRead);
			readerQuotas.MaxNameTableCharCount = Convert.ToInt32(bindingInfo.BindingConfiguration.ReaderQuotas.MaxNameTableCharCount);
			binding.ReaderQuotas = readerQuotas;
			/*Security Configuration*/
			BasicHttpSecurity security = binding.Security;
			security.Mode = BasicHttpSecurityMode.None;
			/*Transport Security Configuration*/
			HttpTransportSecurity transportSecurity =security.Transport;
			transportSecurity.ClientCredentialType = HttpClientCredentialType.None;
			transportSecurity.ProxyCredentialType = HttpProxyCredentialType.None;

			var factory = new ChannelFactory<T>(binding, new EndpointAddress(bindingInfo.EndpointAdress));
			return factory.CreateChannel();
		}
	}
}