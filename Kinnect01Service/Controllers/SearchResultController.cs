using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using Kinnect01Service.Models;
using Kinnect01Service.DataObjects;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
using System;
using System.Reflection;
using System.Data.SqlClient;
using System.Linq;

namespace Kinnect01Service.Controllers
{
    [MobileAppController]
    public class SearchResultController : ApiController
    {
        //Set context for database
        Kinnect01Context context = new Kinnect01Context();

        // GET api/SearchResult
        public List<SearchResult> Get(string ownUserId, string searchType, int resultCount)
        {
            List<SearchResult> searchResults = SearchResultHelper.GetSearchResults(ownUserId, searchType);
            searchResults = searchResults.OrderByDescending(x => x.TotalScore).Take(resultCount).ToList();

            return (searchResults);
        }

        public string Put()
        {
            //Get List of Search Scores & transform into dataTable
            List<SearchScore> searchScores = SearchResultHelper.PopulateSearchScoresTable("Peer");
            DataTable dt = ToDataTable(searchScores);

            //set table name
            string tableName = "SearchScores";

            //save to database
            string _connectionString = context.Database.Connection.ConnectionString;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                //Delete all data in a Table
                string sqlTrunc = "TRUNCATE TABLE " + tableName;
                SqlCommand cmd = new SqlCommand(sqlTrunc, connection);
                cmd.ExecuteNonQuery();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = 5000;
                    bulkCopy.WriteToServer(dt);
                }
            }

            return "SearchScores Table is updated";
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}
