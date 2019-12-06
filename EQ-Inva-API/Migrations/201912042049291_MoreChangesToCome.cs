namespace EQ_Inva_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoreChangesToCome : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Is_Active");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Is_Active", c => c.Boolean(nullable: false));
        }
    }
}
