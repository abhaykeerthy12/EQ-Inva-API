namespace EQ_Inva_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedRequestTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Requests", "Summary", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Requests", "Summary");
        }
    }
}
