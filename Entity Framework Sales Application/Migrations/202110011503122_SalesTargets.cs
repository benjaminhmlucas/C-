namespace SalesApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesTargets : DbMigration
    {
        public override void Up()
        {
            //Default value can be added at the end of inner parenthesis for each column.
            AddColumn("dbo.SalesPersons", "SalesTarget", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 1000.00M));
            AddColumn("dbo.SalesRegions", "SalesTarget", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 10000.00M));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesRegions", "SalesTarget");
            DropColumn("dbo.SalesPersons", "SalesTarget");
        }
    }
}
