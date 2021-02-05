namespace GBW.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "InvitedUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "InvitedUserId");
            AddForeignKey("dbo.AspNetUsers", "InvitedUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "InvitedUserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "InvitedUserId" });
            DropColumn("dbo.AspNetUsers", "InvitedUserId");
        }
    }
}
