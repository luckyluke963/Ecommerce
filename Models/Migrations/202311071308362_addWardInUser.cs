namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWardInUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "WardID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "WardID");
        }
    }
}
