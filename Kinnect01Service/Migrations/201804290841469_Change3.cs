namespace Kinnect01Service.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Name", c => c.Int(nullable: false));
        }
    }
}
