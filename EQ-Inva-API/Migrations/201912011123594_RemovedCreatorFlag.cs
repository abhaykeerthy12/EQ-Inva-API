namespace EQ_Inva_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedCreatorFlag : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Is_Creator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Is_Creator", c => c.Boolean(nullable: false));
        }
    }
}
