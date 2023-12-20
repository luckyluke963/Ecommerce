namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imageInfor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "image");
        }
    }
}
