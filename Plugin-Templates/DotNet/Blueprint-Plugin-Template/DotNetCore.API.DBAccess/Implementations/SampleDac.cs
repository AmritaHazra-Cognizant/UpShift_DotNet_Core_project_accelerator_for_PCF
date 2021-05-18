using DotNetCore.API.DBAccess.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using DotNetCore.API.Contract;
using System.Threading.Tasks;
namespace DotNetCore.API.DBAccess.Implementations
{
    /*
  * This class is used to show sample DataAccess layer example
  * To Do: For Actual project please remove this file 
  * after getting the idea of how to implement.
  */
    public class SampleDac:BaseDac, ISampleDac
    {
        public SampleDac(IServiceProvider serviceProvider) : base(serviceProvider)
        { 
        }

        public async Task<IEnumerable<Student>> GetAllStudent()
        {
            List<Student> lstStudent = new List<Student>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllStudent", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Student student = new Student();
                    student.Id = Convert.ToInt32(rdr["Id"]);
                    student.FirstName = rdr["FirstName"].ToString();
                    student.LastName = rdr["LastName"].ToString();
                    student.Email = rdr["Email"].ToString();
                    student.Mobile = rdr["Mobile"].ToString();
                    student.Address = rdr["Address"].ToString();

                    lstStudent.Add(student);
                }
                con.Close();
            }
            return lstStudent;
        }
    }
}
