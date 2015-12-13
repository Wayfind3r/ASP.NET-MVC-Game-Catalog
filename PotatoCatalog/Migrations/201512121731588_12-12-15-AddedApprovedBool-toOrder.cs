namespace PotatoCatalog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _121215AddedApprovedBooltoOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Approved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Approved");
        }
    }
}
