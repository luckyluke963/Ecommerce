namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTHONGke12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_ThongKe", "ThoiGian", c => c.DateTime(nullable: false));
            DropColumn("dbo.tb_ThongKe", "Time");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_ThongKe", "Time", c => c.DateTime(nullable: false));
            DropColumn("dbo.tb_ThongKe", "ThoiGian");
        }
    }
}
