namespace EQ_Inva_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedProductAndRequestTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Type = c.String(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        EmployeeId = c.String(nullable: false),
                        ProductId = c.String(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Status = c.String(nullable: false),
                        RequestedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Requests");
            DropTable("dbo.Products");
        }
    }
}
