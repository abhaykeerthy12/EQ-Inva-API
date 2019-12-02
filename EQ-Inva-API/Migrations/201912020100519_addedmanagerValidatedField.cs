namespace EQ_Inva_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedmanagerValidatedField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "ManagerValidated", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "ManagerValidated");
        }
    }
}
