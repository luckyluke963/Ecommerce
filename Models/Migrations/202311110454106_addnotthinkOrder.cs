namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnotthinkOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "notthink", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Order", "notthink");
        }
    }
}
