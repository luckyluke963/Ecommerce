namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCodeInSubcribe1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Subscribe", "status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Subscribe", "status");
        }
    }
}
