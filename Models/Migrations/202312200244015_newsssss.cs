namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newsssss : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_News", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.tb_News", "Image", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_News", "Image", c => c.String());
            AlterColumn("dbo.tb_News", "Description", c => c.String());
        }
    }
}
