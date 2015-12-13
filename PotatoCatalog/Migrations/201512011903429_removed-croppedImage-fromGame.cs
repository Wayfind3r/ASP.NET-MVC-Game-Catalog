namespace PotatoCatalog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removedcroppedImagefromGame : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Games", "CroppedImgPath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "CroppedImgPath", c => c.String());
        }
    }
}
