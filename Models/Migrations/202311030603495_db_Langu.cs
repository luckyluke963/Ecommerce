namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db_Langu : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tb_Language",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 2),
                        Name = c.String(maxLength: 50),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tb_Language");
        }
    }
}
