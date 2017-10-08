namespace ExpenseLink.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequestReceiptAndStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Receipts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReceiptDate = c.DateTime(nullable: false),
                        ItemDescription = c.String(),
                        Amount = c.Double(nullable: false),
                        ReimbursementAmount = c.Double(nullable: false),
                        Request_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Requests", t => t.Request_Id)
                .Index(t => t.Request_Id);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Requests", "CreatedBy_Id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Requests", "Status_Id", c => c.Byte());
            CreateIndex("dbo.Requests", "CreatedBy_Id");
            CreateIndex("dbo.Requests", "Status_Id");
            AddForeignKey("dbo.Requests", "CreatedBy_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Requests", "Status_Id", "dbo.Status", "Id");
            DropColumn("dbo.Requests", "ReceiptId");
            DropColumn("dbo.Requests", "StatusId");
            DropColumn("dbo.Requests", "CreatedByEmployeeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Requests", "CreatedByEmployeeId", c => c.Int(nullable: false));
            AddColumn("dbo.Requests", "StatusId", c => c.Byte(nullable: false));
            AddColumn("dbo.Requests", "ReceiptId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Requests", "Status_Id", "dbo.Status");
            DropForeignKey("dbo.Receipts", "Request_Id", "dbo.Requests");
            DropForeignKey("dbo.Requests", "CreatedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "Status_Id" });
            DropIndex("dbo.Requests", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Receipts", new[] { "Request_Id" });
            DropColumn("dbo.Requests", "Status_Id");
            DropColumn("dbo.Requests", "CreatedBy_Id");
            DropTable("dbo.Status");
            DropTable("dbo.Receipts");
        }
    }
}
