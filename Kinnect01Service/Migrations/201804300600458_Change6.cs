namespace Kinnect01Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change6 : DbMigration
    {
        public override void Up()
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SearchScores");
        }
    }
}
