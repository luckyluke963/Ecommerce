namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newsssssa : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_News", "Detail", c => c.String(nullable: false));
            DropColumn("dbo.tb_News", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_News", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.tb_News", "Detail", c => c.String());
        }
    }
}
