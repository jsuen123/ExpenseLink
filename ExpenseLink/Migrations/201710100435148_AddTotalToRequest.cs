namespace ExpenseLink.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTotalToRequest : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "Total", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "Total");
        }
    }
}
