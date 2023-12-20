namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAcount112 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "FullName", c => c.String());
            AddColumn("dbo.User", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "Phone");
            DropColumn("dbo.User", "FullName");
        }
    }
}
