namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductCategoryUpdate33 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_ProductCategory", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_ProductCategory", "Status", c => c.Boolean());
        }
    }
}
