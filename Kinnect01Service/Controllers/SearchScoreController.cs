using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using Kinnect01Service.DataObjects;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kinnect01Service.Models;
using Microsoft.Azure.Mobile.Server;
using System.Web.Http.Controllers;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Azure;

namespace Kinnect01Service.Controllers
{
    [MobileAppController]
    public class SearchScoreController : TableController<TotalScore>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            Kinnect01Context context = new Kinnect01Context();
            DomainManager = new EntityDomainManager<TotalScore>(context, Request);
        }

        // GET api/SearchResult
        //public List<SearchResult> Get(string ownUserId, string searchType)
        //{
        //    return (SearchResultHelper.GetSearchResults(ownUserId, searchType));
        //}

        //PUT api/SearchResult
        //public string Put()
        //{
        //    List<SearchScore> searchScores = SearchResultHelper.PopulateSearchScoresTable("Peer");

        //    //Insert into database
        //    Kinnect01Context context = new Kinnect01Context();
            
        //    //create weird table
        //    var dt = new DataTable();
        //    dt.Columns.Add("EmployeeID");
        //    dt.Columns.Add("Name");

        //    for (var i = 1; i < 1000000; i++)
        //        dt.Rows.Add(i + 1, "Name " + i + 1);

        //    //add it now
        //    string _connectionString = context.Database.Connection.ConnectionString;
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        //Delete all data in a Table
        //        string sqlTrunc = "TRUNCATE TABLE " + "Employees";
        //        SqlCommand cmd = new SqlCommand(sqlTrunc, connection);
        //        cmd.ExecuteNonQuery();

        //        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
        //        {
        //            bulkCopy.DestinationTableName = "Employees";
        //            bulkCopy.WriteToServer(dt);
        //        }
        //    }

        //    //context.Configuration.AutoDetectChangesEnabled = false;

        //    //int count = 0;
        //    //foreach (SearchScore item in searchScores)
        //    //{
        //    //    count++;
        //    //    context = await AddToContextAsync(context, item, count, 10000, true);                
        //    //}


        //    return "Ok";
        //}

        private async Task<Kinnect01Context> AddToContextAsync(Kinnect01Context context, TotalScore item, 
                                               int count, int commitCount, bool recreateContext)
        {
            TotalScore current = await InsertAsync(item);

            if (count % commitCount == 0)
            {
                context.SaveChanges();
                if (recreateContext)
                {
                    context.Dispose();
                    context = new Kinnect01Context();
                    context.Configuration.AutoDetectChangesEnabled = false;
                }
            }
            return context;
        }
    }
}