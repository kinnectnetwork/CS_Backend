namespace Kinnect01Service.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Change4 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.SearchScores", new[] { "CreatedAt" });
            DropPrimaryKey("dbo.SearchScores");

            DropColumn("dbo.SearchScores", "Version",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "Version" },
                });
            DropColumn("dbo.SearchScores", "CreatedAt",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "CreatedAt" },
                });
            DropColumn("dbo.SearchScores", "UpdatedAt",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "UpdatedAt" },
                });
            DropColumn("dbo.SearchScores", "Deleted",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "ServiceTableColumn", "Deleted" },
                });
        }
        
        public override void Down()
        {
            AddColumn("dbo.SearchScores", "Deleted", c => c.Boolean(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "Deleted")
                    },
                }));
            AddColumn("dbo.SearchScores", "UpdatedAt", c => c.DateTimeOffset(precision: 7,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                    },
                }));
            AddColumn("dbo.SearchScores", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                    },
                }));
            AddColumn("dbo.SearchScores", "Version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "Version")
                    },
                }));
            DropPrimaryKey("dbo.SearchScores");
            CreateIndex("dbo.SearchScores", "CreatedAt", clustered: true);
        }
    }
}
