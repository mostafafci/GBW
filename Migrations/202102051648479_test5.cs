namespace GBW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Parteners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                        WebsiteURL = c.String(),
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
            DropForeignKey("dbo.Parteners", "AddedBy", "dbo.AspNetUsers");
            DropIndex("dbo.Parteners", new[] { "AddedBy" });
            DropTable("dbo.Parteners");
        }
    }
}
