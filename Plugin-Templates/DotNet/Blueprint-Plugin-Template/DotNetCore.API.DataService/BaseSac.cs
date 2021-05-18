using DotNetCore.Framework.Logging.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotNetCore.API.DataService
{
    public class BaseSac
    {
        public BaseSac(IServiceProvider serviceProvider)
        {
            LogEntry = serviceProvider.GetRequiredService<TransactionLogEntry>(); ;
            LockObject = new object();
            
        }

        public TransactionLogEntry LogEntry { get; set; }
        public object LockObject { get; set; }
    }

   
}
