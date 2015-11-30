namespace Typespot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUsers", "ImageUrl");
        }
    }
}
