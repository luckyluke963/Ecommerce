namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAccountCreateDay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "CreatetAcount", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "CreatetAcount");
        }
    }
}
