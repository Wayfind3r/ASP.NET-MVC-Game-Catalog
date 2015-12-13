namespace PotatoCatalog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedimagepathtoGame : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "ImgPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "ImgPath");
        }
    }
}
