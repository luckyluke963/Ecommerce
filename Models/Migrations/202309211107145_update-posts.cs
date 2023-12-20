namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateposts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tb_Posts", "Category_Id", "dbo.tb_Category");
            DropIndex("dbo.tb_Posts", new[] { "Category_Id" });
            DropColumn("dbo.tb_Posts", "CategoryId");
            RenameColumn(table: "dbo.tb_Posts", name: "Category_Id", newName: "CategoryId");
            AlterColumn("dbo.tb_Posts", "Alias", c => c.String(maxLength: 150));
            AlterColumn("dbo.tb_Posts", "Image", c => c.String(maxLength: 250));
            AlterColumn("dbo.tb_Posts", "CategoryId", c => c.Int(nullable: false));
            AlterColumn("dbo.tb_Posts", "SeoTitle", c => c.String(maxLength: 250));
            AlterColumn("dbo.tb_Posts", "SeoDescription", c => c.String(maxLength: 250));
            AlterColumn("dbo.tb_Posts", "SeoKeywords", c => c.String(maxLength: 250));
            AlterColumn("dbo.tb_Posts", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.tb_Posts", "CategoryId");
            AddForeignKey("dbo.tb_Posts", "CategoryId", "dbo.tb_Category", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Posts", "CategoryId", "dbo.tb_Category");
            DropIndex("dbo.tb_Posts", new[] { "CategoryId" });
            AlterColumn("dbo.tb_Posts", "CategoryId", c => c.Int());
            AlterColumn("dbo.tb_Posts", "SeoKeywords", c => c.String());
            AlterColumn("dbo.tb_Posts", "SeoDescription", c => c.String());
            AlterColumn("dbo.tb_Posts", "SeoTitle", c => c.String());
            AlterColumn("dbo.tb_Posts", "CategoryId", c => c.String());
            AlterColumn("dbo.tb_Posts", "Image", c => c.String());
            AlterColumn("dbo.tb_Posts", "Alias", c => c.String());
            RenameColumn(table: "dbo.tb_Posts", name: "CategoryId", newName: "Category_Id");
            AddColumn("dbo.tb_Posts", "CategoryId", c => c.String());
            CreateIndex("dbo.tb_Posts", "Category_Id");
            AddForeignKey("dbo.tb_Posts", "Category_Id", "dbo.tb_Category", "Id");
        }
    }
}
