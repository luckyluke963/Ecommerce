namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateViewCountInProduct : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Product", "ViewCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Product", "ViewCount", c => c.Int());
        }
    }
}
