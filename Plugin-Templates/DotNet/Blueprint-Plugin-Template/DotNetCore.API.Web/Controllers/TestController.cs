using DotNetCore.Framework.Logging.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.API.BusinessLogic.Contracts;
using DotNetCore.API.Contract;
namespace DotNetCore.API.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : BaseApiController
    {
        private readonly ISampleBc _sampleBc;
        public TestController(TransactionLogEntry logEntry,ISampleBc sampleBc) : base(logEntry)
        {
            _sampleBc = sampleBc;
        }
        [HttpGet("getString")]
        public async Task<string> GetString()
        {
            return "Hello User!";
        }

        /// <summary>
        /// This API is calling Backend WCF SOAP Service
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetStringFromSoapService")]
        public async Task<string> GetStringFromSoapService()
        {
            var result = await _sampleBc.GetHellowWorldFromSoapService();
            return result;
        }

        /// <summary>
        /// This API is calling Backend Database
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllStudentFromDB")]
        public async Task<IEnumerable<Student>> GetAllStudent()
        {
            var result = await _sampleBc.GetAllStudent();
            return result;
        }

        /// <summary>
        /// This API is calling Backend REST API
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetEmployeesFromRestApi")]
        public async Task<IEnumerable<EmployeeInfo>> GetEmployeesFromRestApi()
        {
            var result = await _sampleBc.RetrieveEmployeesFromRestApi();
            return result.Model;
        }

    }
}
