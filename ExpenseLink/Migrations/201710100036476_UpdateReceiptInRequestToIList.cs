namespace ExpenseLink.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReceiptInRequestToIList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Receipts", "Request_Id", c => c.Int());
            CreateIndex("dbo.Receipts", "Request_Id");
            AddForeignKey("dbo.Receipts", "Request_Id", "dbo.Requests", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Receipts", "Request_Id", "dbo.Requests");
            DropIndex("dbo.Receipts", new[] { "Request_Id" });
            DropColumn("dbo.Receipts", "Request_Id");
        }
    }
}
