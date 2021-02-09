namespace GBW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GoldPackageEducationLinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EduLink = c.String(),
                        GoldPackage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoldPackages", t => t.GoldPackage_Id)
                .Index(t => t.GoldPackage_Id);
            
            CreateTable(
                "dbo.GoldPackages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                        NumberOfGram = c.Double(nullable: false),
                        Value = c.Double(nullable: false),
                        RevenuePerMonth = c.Double(nullable: false),
                        NumberOfCards = c.Int(nullable: false),
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
            DropForeignKey("dbo.GoldPackages", "AddedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.GoldPackageEducationLinks", "GoldPackage_Id", "dbo.GoldPackages");
            DropIndex("dbo.GoldPackages", new[] { "AddedBy" });
            DropIndex("dbo.GoldPackageEducationLinks", new[] { "GoldPackage_Id" });
            DropTable("dbo.GoldPackages");
            DropTable("dbo.GoldPackageEducationLinks");
        }
    }
}
