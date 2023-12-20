namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OderAdderss : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "ProvinceID", c => c.Int());
            AddColumn("dbo.tb_Order", "DistrictID", c => c.Int());
            AddColumn("dbo.tb_Order", "WardID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Order", "WardID");
            DropColumn("dbo.tb_Order", "DistrictID");
            DropColumn("dbo.tb_Order", "ProvinceID");
        }
    }
}
