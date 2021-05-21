using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNetCore.API.Contract;
namespace DotNetCore.API.DataService.Contracts
{
    /*
     * This class is used to show sample Service layer example
     * To Do: For Actual project please remove this file 
     * after getting the idea of how to implement.
     */
    public interface ISampleSac
    {
        Task<string> RetrieveHellowWorld();

        Task<List<EmployeeInfo>> RetrieveEmployees();
    }
}
