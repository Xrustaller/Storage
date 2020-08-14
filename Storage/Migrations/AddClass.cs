using System.Data.Entity.Migrations;

namespace Storage.Migrations
{
    public class AddClass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accepted",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    NameItem = c.String(maxLength: 200),
                    Count = c.Int(),
                    Price = c.Int(),
                    DateCreate = c.DateTime(),
                });
            CreateTable(
                "dbo.SoldOut",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    NameItem = c.String(maxLength: 200),
                    Count = c.Int(),
                    Price = c.Int(),
                    DateCreate = c.DateTime(),
                });
            CreateTable(
                "dbo.Storage",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    NameItem = c.String(maxLength: 200),
                    Count = c.Int(),
                    Price = c.Int(),
                    DateCreate = c.DateTime(),
                });
        }
    }
}
