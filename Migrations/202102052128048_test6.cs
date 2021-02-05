namespace GBW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        AddedBy = c.String(maxLength: 128),
                        AddedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.AddedBy)
                .Index(t => t.AddedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "AddedBy", "dbo.AspNetUsers");
            DropIndex("dbo.Events", new[] { "AddedBy" });
            DropTable("dbo.Events");
        }
    }
}
