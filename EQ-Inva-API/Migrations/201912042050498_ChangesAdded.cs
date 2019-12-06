namespace EQ_Inva_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Is_Active", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Is_Active");
        }
    }
}
