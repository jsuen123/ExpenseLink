namespace ExpenseLink.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusIdToRequest : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requests", "Status_Id", "dbo.Status");
            DropIndex("dbo.Requests", new[] { "Status_Id" });
            RenameColumn(table: "dbo.Requests", name: "Status_Id", newName: "StatusId");
            AlterColumn("dbo.Requests", "StatusId", c => c.Byte(nullable: false));
            CreateIndex("dbo.Requests", "StatusId");
            AddForeignKey("dbo.Requests", "StatusId", "dbo.Status", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "StatusId", "dbo.Status");
            DropIndex("dbo.Requests", new[] { "StatusId" });
            AlterColumn("dbo.Requests", "StatusId", c => c.Byte());
            RenameColumn(table: "dbo.Requests", name: "StatusId", newName: "Status_Id");
            CreateIndex("dbo.Requests", "Status_Id");
            AddForeignKey("dbo.Requests", "Status_Id", "dbo.Status", "Id");
        }
    }
}
