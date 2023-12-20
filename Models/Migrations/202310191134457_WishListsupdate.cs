namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WishListsupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Wishlist", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tb_Wishlist", "UserName");
        }
    }
}
