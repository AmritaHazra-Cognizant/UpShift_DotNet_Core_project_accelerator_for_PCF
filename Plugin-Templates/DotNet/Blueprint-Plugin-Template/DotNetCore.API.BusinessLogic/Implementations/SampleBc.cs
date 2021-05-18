using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNetCore.API.BusinessLogic.Contracts;
using DotNetCore.API.DataService.Contracts;
using DotNetCore.API.DBAccess.Contracts;
using DotNetCore.API.Contract;
namespace DotNetCore.API.BusinessLogic.Implementations
{
    public class SampleBc : ISampleBc
    {
        private readonly ISampleSac _service;
        private readonly ISampleDac _database;
        public SampleBc(ISampleSac sampleSac, ISampleDac sampleDac)
        {
            _service = sampleSac;
            _database = sampleDac;
        }

        public async Task<string> GetHellowWorldFromSoapService()
        {
            var result = await _service.RetrieveHellowWorld();
            return result;
        }
        public async Task<IEnumerable<Student>> GetAllStudent()
        {
            var result = await _database.GetAllStudent();
            return result;
        }
    }
}
