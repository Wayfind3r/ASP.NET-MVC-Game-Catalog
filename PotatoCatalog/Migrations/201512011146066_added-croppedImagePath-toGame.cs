namespace PotatoCatalog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedcroppedImagePathtoGame : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "CroppedImgPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "CroppedImgPath");
        }
    }
}
