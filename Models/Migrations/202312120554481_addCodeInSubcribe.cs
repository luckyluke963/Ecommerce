namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCodeInSubcribe : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Subscribe", "MaCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Subscribe", "MaCode");
        }
    }
}
