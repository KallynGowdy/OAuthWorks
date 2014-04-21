namespace ExampleWebApiProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthorizationCodes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        RedirectUri = c.String(),
                        ExpirationDateUtc = c.DateTime(nullable: false),
                        Client_Name = c.String(maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Code)
                .ForeignKey("dbo.Clients", t => t.Client_Name)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Client_Name)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Secret = c.String(),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.RedirectUris",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Uri = c.String(),
                        Client_Name = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Name)
                .Index(t => t.Client_Name);
            
            CreateTable(
                "dbo.Scopes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScopeAuthorizationCodes",
                c => new
                    {
                        Scope_Id = c.String(nullable: false, maxLength: 128),
                        AuthorizationCode_Code = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Scope_Id, t.AuthorizationCode_Code })
                .ForeignKey("dbo.Scopes", t => t.Scope_Id, cascadeDelete: true)
                .ForeignKey("dbo.AuthorizationCodes", t => t.AuthorizationCode_Code, cascadeDelete: true)
                .Index(t => t.Scope_Id)
                .Index(t => t.AuthorizationCode_Code);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthorizationCodes", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ScopeAuthorizationCodes", "AuthorizationCode_Code", "dbo.AuthorizationCodes");
            DropForeignKey("dbo.ScopeAuthorizationCodes", "Scope_Id", "dbo.Scopes");
            DropForeignKey("dbo.AuthorizationCodes", "Client_Name", "dbo.Clients");
            DropForeignKey("dbo.RedirectUris", "Client_Name", "dbo.Clients");
            DropIndex("dbo.ScopeAuthorizationCodes", new[] { "AuthorizationCode_Code" });
            DropIndex("dbo.ScopeAuthorizationCodes", new[] { "Scope_Id" });
            DropIndex("dbo.RedirectUris", new[] { "Client_Name" });
            DropIndex("dbo.AuthorizationCodes", new[] { "User_Id" });
            DropIndex("dbo.AuthorizationCodes", new[] { "Client_Name" });
            DropTable("dbo.ScopeAuthorizationCodes");
            DropTable("dbo.Users");
            DropTable("dbo.Scopes");
            DropTable("dbo.RedirectUris");
            DropTable("dbo.Clients");
            DropTable("dbo.AuthorizationCodes");
        }
    }
}
