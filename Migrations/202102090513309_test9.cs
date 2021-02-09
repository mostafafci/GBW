namespace GBW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GoldPackageEducationLinks", "GoldPackage_Id", "dbo.GoldPackages");
            DropIndex("dbo.GoldPackageEducationLinks", new[] { "GoldPackage_Id" });
            AddColumn("dbo.GoldPackageEducationLinks", "GoldPackage_Id1", c => c.Int());
            AlterColumn("dbo.GoldPackageEducationLinks", "GoldPackage_Id", c => c.String());
            CreateIndex("dbo.GoldPackageEducationLinks", "GoldPackage_Id1");
            AddForeignKey("dbo.GoldPackageEducationLinks", "GoldPackage_Id1", "dbo.GoldPackages", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoldPackageEducationLinks", "GoldPackage_Id1", "dbo.GoldPackages");
            DropIndex("dbo.GoldPackageEducationLinks", new[] { "GoldPackage_Id1" });
            AlterColumn("dbo.GoldPackageEducationLinks", "GoldPackage_Id", c => c.Int());
            DropColumn("dbo.GoldPackageEducationLinks", "GoldPackage_Id1");
            CreateIndex("dbo.GoldPackageEducationLinks", "GoldPackage_Id");
            AddForeignKey("dbo.GoldPackageEducationLinks", "GoldPackage_Id", "dbo.GoldPackages", "Id");
        }
    }
}
