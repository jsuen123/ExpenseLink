namespace ExpenseLink.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReceiptNumberToReceipt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Receipts", "ReceiptNo", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Receipts", "ReceiptNo");
        }
    }
}
