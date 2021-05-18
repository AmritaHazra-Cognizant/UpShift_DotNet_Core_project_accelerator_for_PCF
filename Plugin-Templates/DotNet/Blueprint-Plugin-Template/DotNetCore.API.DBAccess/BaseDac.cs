using DotNetCore.Framework.Logging.Interface;
using DotNetCore.Framework.Logging.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotNetCore.API.DBAccess
{
    public class BaseDac : IBaseHandler
    {
        private readonly string _defaultConnectionString;
        public BaseDac(IServiceProvider serviceProvider)
        {
            LogEntry = serviceProvider.GetRequiredService<TransactionLogEntry>(); ;
            LockObject = new object();
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            // read from Appsettings
            _defaultConnectionString = config["ConnectionStrings:DefaultConnection"];
        }
        public TransactionLogEntry LogEntry { get; set; }
        public object LockObject { get; set; }
        protected string ConnectionString { get { return _defaultConnectionString; } }
    }
}
