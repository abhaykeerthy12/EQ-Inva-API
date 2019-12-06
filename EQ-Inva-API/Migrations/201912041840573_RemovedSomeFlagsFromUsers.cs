namespace EQ_Inva_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedSomeFlagsFromUsers : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Is_Admin");
            DropColumn("dbo.AspNetUsers", "Is_Manager");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Is_Manager", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "Is_Admin", c => c.Boolean(nullable: false));
        }
    }
}
