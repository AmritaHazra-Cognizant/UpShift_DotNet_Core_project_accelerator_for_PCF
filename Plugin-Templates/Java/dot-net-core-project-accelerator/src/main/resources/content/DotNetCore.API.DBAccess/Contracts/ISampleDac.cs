using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNetCore.API.Contract;
namespace DotNetCore.API.DBAccess.Contracts
{
    /*
     * This class is used to show sample DBAccess layer example
     * To Do: For Actual project please remove this file 
     * after getting the idea of how to implement.
     */
    public interface ISampleDac
    {
        Task<IEnumerable<Student>> GetAllStudent();
    }
}
