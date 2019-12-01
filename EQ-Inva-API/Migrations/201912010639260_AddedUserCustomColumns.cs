namespace EQ_Inva_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserCustomColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Department", c => c.String());
            AddColumn("dbo.AspNetUsers", "Is_Admin", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.AspNetUsers", "Is_Manager", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.AspNetUsers", "Is_Creator", c => c.Boolean(nullable: false, defaultValue: false));
            AddColumn("dbo.AspNetUsers", "Is_Active", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Is_Active");
            DropColumn("dbo.AspNetUsers", "Is_Creator");
            DropColumn("dbo.AspNetUsers", "Is_Manager");
            DropColumn("dbo.AspNetUsers", "Is_Admin");
            DropColumn("dbo.AspNetUsers", "Department");
            DropColumn("dbo.AspNetUsers", "Name");
        }
    }
}
