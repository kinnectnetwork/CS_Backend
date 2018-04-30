using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using Kinnect01Service.DataObjects;

namespace Kinnect01Service.Models
{
    public class Kinnect01Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        private const string connectionStringName = "Name=MS_TableConnectionString";

        public Kinnect01Context() : base(connectionStringName)
        {
        } 

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
        }

        public System.Data.Entity.DbSet<Kinnect01Service.DataObjects.UserProfile> UserProfiles { get; set; }

        public System.Data.Entity.DbSet<Kinnect01Service.DataObjects.AgeScore> AgeScores { get; set; }
        public System.Data.Entity.DbSet<Kinnect01Service.DataObjects.GenderScore> GenderScores { get; set; }
        public System.Data.Entity.DbSet<Kinnect01Service.DataObjects.IndustryScore> IndustryScores { get; set; }
        public System.Data.Entity.DbSet<Kinnect01Service.DataObjects.JobLevelScore> JobLevelScores { get; set; }
        public System.Data.Entity.DbSet<Kinnect01Service.DataObjects.LocationScore> LocationScores { get; set; }
        public System.Data.Entity.DbSet<Kinnect01Service.DataObjects.OrganisationTypeScore> OrganisationTypeScores { get; set; }
        public System.Data.Entity.DbSet<Kinnect01Service.DataObjects.TotalScore> TotalScores { get; set; }

        public System.Data.Entity.DbSet<Kinnect01Service.DataObjects.JobLevelMapping> JobLevelMappings { get; set; }

        public System.Data.Entity.DbSet<Kinnect01Service.DataObjects.SearchScore> SearchScores { get; set; }

        public System.Data.Entity.DbSet<Kinnect01Service.DataObjects.Employee> Employees { get; set; }

    }

}
