namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductCategoryLanguage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_ProductCategory", "Language", c => c.String(maxLength: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_ProductCategory", "Language");
        }
    }
}
