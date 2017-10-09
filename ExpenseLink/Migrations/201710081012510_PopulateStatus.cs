namespace ExpenseLink.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateStatus : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO STATUS (Id, Name) VALUES (1, 'Submitted')");
            Sql("INSERT INTO STATUS (Id, Name) VALUES (2, 'Rejected')");
            Sql("INSERT INTO STATUS (Id, Name) VALUES (3, 'Waiting for reimbursement')");
            Sql("INSERT INTO STATUS (Id, Name) VALUES (4, 'Reimbursed')");
        }
        
        public override void Down()
        {
        }
    }
}
