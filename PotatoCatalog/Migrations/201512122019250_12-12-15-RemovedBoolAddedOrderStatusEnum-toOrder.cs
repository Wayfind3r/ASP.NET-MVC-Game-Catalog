namespace PotatoCatalog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _121215RemovedBoolAddedOrderStatusEnumtoOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "OrderStatusId", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "OrderStatus", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "Approved");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Approved", c => c.Boolean(nullable: false));
            DropColumn("dbo.Orders", "OrderStatus");
            DropColumn("dbo.Orders", "OrderStatusId");
        }
    }
}
