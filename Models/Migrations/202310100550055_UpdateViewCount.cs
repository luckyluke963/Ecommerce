namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateViewCount : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Statistics", newName: "tb_ThongKe");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.tb_ThongKe", newName: "Statistics");
        }
    }
}
