namespace ExpenseLink.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateRequestModel : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Requests", name: "CreatedBy_Id", newName: "ApplicationUser_Id");
            RenameIndex(table: "dbo.Requests", name: "IX_CreatedBy_Id", newName: "IX_ApplicationUser_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Requests", name: "IX_ApplicationUser_Id", newName: "IX_CreatedBy_Id");
            RenameColumn(table: "dbo.Requests", name: "ApplicationUser_Id", newName: "CreatedBy_Id");
        }
    }
}
