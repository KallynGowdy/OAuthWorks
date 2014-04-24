namespace ExampleWebApiProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedHashedValues : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HashedValues",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Hash = c.String(),
                        Salt = c.String(),
                        HashIterations = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "Password_Id", c => c.Long());
            CreateIndex("dbo.Users", "Password_Id");
            AddForeignKey("dbo.Users", "Password_Id", "dbo.HashedValues", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Password_Id", "dbo.HashedValues");
            DropIndex("dbo.Users", new[] { "Password_Id" });
            DropColumn("dbo.Users", "Password_Id");
            DropTable("dbo.HashedValues");
        }
    }
}
