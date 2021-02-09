namespace GBW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test91 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.GoldPackageEducationLinks", new[] { "GoldPackage_Id1" });
            DropColumn("dbo.GoldPackageEducationLinks", "GoldPackage_Id");
            RenameColumn(table: "dbo.GoldPackageEducationLinks", name: "GoldPackage_Id1", newName: "GoldPackage_Id");
            AlterColumn("dbo.GoldPackageEducationLinks", "GoldPackage_Id", c => c.Int());
            CreateIndex("dbo.GoldPackageEducationLinks", "GoldPackage_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.GoldPackageEducationLinks", new[] { "GoldPackage_Id" });
            AlterColumn("dbo.GoldPackageEducationLinks", "GoldPackage_Id", c => c.String());
            RenameColumn(table: "dbo.GoldPackageEducationLinks", name: "GoldPackage_Id", newName: "GoldPackage_Id1");
            AddColumn("dbo.GoldPackageEducationLinks", "GoldPackage_Id", c => c.String());
            CreateIndex("dbo.GoldPackageEducationLinks", "GoldPackage_Id1");
        }
    }
}
