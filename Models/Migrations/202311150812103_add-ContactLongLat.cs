namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addContactLongLat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Contact", "Long", c => c.Double(nullable: false));
            AddColumn("dbo.tb_Contact", "Lat", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Contact", "Lat");
            DropColumn("dbo.tb_Contact", "Long");
        }
    }
}
