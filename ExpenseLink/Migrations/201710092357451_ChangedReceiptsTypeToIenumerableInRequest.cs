namespace ExpenseLink.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedReceiptsTypeToIenumerableInRequest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Receipts", "Request_Id", "dbo.Requests");
            DropIndex("dbo.Receipts", new[] { "Request_Id" });
            DropColumn("dbo.Receipts", "Request_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Receipts", "Request_Id", c => c.Int());
            CreateIndex("dbo.Receipts", "Request_Id");
            AddForeignKey("dbo.Receipts", "Request_Id", "dbo.Requests", "Id");
        }
    }
}
