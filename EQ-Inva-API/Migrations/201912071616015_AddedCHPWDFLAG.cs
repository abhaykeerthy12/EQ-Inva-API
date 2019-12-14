namespace EQ_Inva_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCHPWDFLAG : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ChangedPwd", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ChangedPwd");
        }
    }
}
