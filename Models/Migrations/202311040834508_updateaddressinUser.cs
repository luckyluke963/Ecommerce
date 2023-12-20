namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateaddressinUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Address", c => c.String());
            AddColumn("dbo.User", "ProvinceID", c => c.Int());
            AddColumn("dbo.User", "DistrictID", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "DistrictID");
            DropColumn("dbo.User", "ProvinceID");
            DropColumn("dbo.User", "Address");
        }
    }
}
