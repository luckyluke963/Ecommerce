namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSlide : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Slide", "Status", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Slide", "Status", c => c.Boolean());
        }
    }
}
