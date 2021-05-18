using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNetCore.API.Contract;
namespace DotNetCore.API.BusinessLogic.Contracts
{
    public interface ISampleBc
    {
        Task<string> GetHellowWorldFromSoapService();
        Task<IEnumerable<Student>> GetAllStudent();
    }
}
