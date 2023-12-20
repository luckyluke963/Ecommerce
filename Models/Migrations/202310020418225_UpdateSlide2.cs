namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSlide2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Slide", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Slide", "CreateDate", c => c.DateTime());
        }
    }
}
