namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatenews : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.tb_News", "CateogoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_News", "CateogoryId", c => c.Int(nullable: false));
        }
    }
}
