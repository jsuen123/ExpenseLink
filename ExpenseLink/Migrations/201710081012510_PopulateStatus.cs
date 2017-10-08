namespace ExpenseLink.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateStatus : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO STATUS (Id, Name) VALUES (1, 'Submitted')");
            Sql("INSERT INTO STATUS (Id, Name) VALUES (2, 'Approved')");
            Sql("INSERT INTO STATUS (Id, Name) VALUES (3, 'Rejected')");
            Sql("INSERT INTO STATUS (Id, Name) VALUES (4, 'Waiting for reimbursement')");
            Sql("INSERT INTO STATUS (Id, Name) VALUES (5, 'Reimbursed')");
        }
        
        public override void Down()
        {
        }
    }
}
