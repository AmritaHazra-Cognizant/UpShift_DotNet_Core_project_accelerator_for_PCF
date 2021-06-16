
/*
  * This class is used to show sample API layer example
  * To Do: For Actual project please remove this file 
  * after getting the idea of how to implement.
  */



using DotNetCore.API.DataService.Providers.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace DotNetCore.API.DataService.Providers.Api
{

    /// <summary>
    /// This Class will behave like proxy class for REST API
    /// </summary>
    public interface IEmployeeRestApi
    {
        Task<EmployeeResponse> ListEmployeeDataAsync();
    }
}
