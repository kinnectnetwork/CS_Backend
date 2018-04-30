using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using Kinnect01Service.Models;
using System.Data;
using System.Data.SqlClient;

namespace Kinnect01Service.Controllers
{
    [MobileAppController]
    public class DefaultController : ApiController
    {
        // GET api/Default
        public string Get()
        {
            return "Hello from custom controller!";
        }

        //PUT api/Default
        public string Put()
        {
            //Insert into database
            Kinnect01Context context = new Kinnect01Context();

            //create weird table
            var dt = new DataTable();
            dt.Columns.Add("EmployeeID");
            dt.Columns.Add("Name");

            for (var i = 1; i < 100000; i++)
                dt.Rows.Add(i + 1, "Name " + i + 1);

            //add it now
            string _connectionString = context.Database.Connection.ConnectionString;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                //Delete all data in a Table
                string sqlTrunc = "TRUNCATE TABLE " + "Employees";
                SqlCommand cmd = new SqlCommand(sqlTrunc, connection);
                cmd.ExecuteNonQuery();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "Employees";
                    bulkCopy.BatchSize = 5000;
                    bulkCopy.WriteToServer(dt);
                }
            }

            return "Ok";
        }

    }
}
