namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateslide23 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tb_Slide", "Description", c => c.String(maxLength: 150));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tb_Slide", "Description", c => c.String(maxLength: 50));
        }
    }
}
