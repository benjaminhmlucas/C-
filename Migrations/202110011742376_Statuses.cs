namespace SalesApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Statuses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sales", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sales", "Status");
        }
    }
}
