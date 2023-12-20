namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class language : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Category", "Language", c => c.String(maxLength: 2));
            AddColumn("dbo.tb_Content", "Language", c => c.String(maxLength: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Content", "Language");
            DropColumn("dbo.tb_Category", "Language");
        }
    }
}
