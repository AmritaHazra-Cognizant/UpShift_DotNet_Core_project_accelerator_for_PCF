using System;

namespace DotNetCore.Framework.WebServices.Configuration
{
	public class ReaderQuotas
	{
		public string MaxDepth { get; set; }
		public string MaxStringContentLength { get; set; }
		public string MaxArrayLength { get; set; }
		public string MaxBytesPerRead { get; set; }
		public string MaxNameTableCharCount { get; set; }
	}
}