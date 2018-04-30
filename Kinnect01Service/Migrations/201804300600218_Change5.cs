namespace Kinnect01Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change5 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Employees");
            DropTable("dbo.SearchScores");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SearchScores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnUserId = c.String(),
                        TargetUserId = c.String(),
                        SearchType = c.String(),
                        Distance = c.Double(nullable: false),
                        GenderScore = c.Double(nullable: false),
                        AgeScore = c.Double(nullable: false),
                        LocationScore = c.Double(nullable: false),
                        IndustryScore = c.Double(nullable: false),
                        OrganisationTypeScore = c.Double(nullable: false),
                        JobLevelScore = c.Double(nullable: false),
                        TotalScore = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.EmployeeID);
            
        }
    }
}
