using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.API.Contract
{
    public class EmployeeInfo
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
    }
}
