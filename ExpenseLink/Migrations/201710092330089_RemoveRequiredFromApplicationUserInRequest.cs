namespace ExpenseLink.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRequiredFromApplicationUserInRequest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "ApplicationUser_Id" });
            AlterColumn("dbo.Requests", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Requests", "ApplicationUser_Id");
            AddForeignKey("dbo.Requests", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "ApplicationUser_Id" });
            AlterColumn("dbo.Requests", "ApplicationUser_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Requests", "ApplicationUser_Id");
            AddForeignKey("dbo.Requests", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
