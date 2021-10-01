namespace SalesApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameSalesPersonsTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SalesPersons", newName: "SalesPeople");
            RenameColumn(table: "dbo.SalesPeople", name: "SalesTarget", newName: "Target");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.SalesPeople", name: "Target", newName: "SalesTarget");
            RenameTable(name: "dbo.SalesPeople", newName: "SalesPersons");
        }
    }
}
