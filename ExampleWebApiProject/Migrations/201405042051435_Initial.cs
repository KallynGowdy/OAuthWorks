namespace ExampleWebApiProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        TokenType = c.String(),
                        ExpirationDateUtc = c.DateTime(nullable: false),
                        Revoked = c.Boolean(nullable: false),
                        Client_Name = c.String(maxLength: 128),
                        TokenValue_Id = c.Long(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Name)
                .ForeignKey("dbo.HashedValues", t => t.TokenValue_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Client_Name)
                .Index(t => t.TokenValue_Id)
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
                        AccessToken_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccessTokens", t => t.AccessToken_Id)
                .Index(t => t.AccessToken_Id);
            
            CreateTable(
                "dbo.AuthorizationCodes",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 128),
                        RedirectUri = c.String(),
                        ExpirationDateUtc = c.DateTime(nullable: false),
                        Revoked = c.Boolean(nullable: false),
                        Client_Name = c.String(maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Code)
                .ForeignKey("dbo.Clients", t => t.Client_Name)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Client_Name)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Password_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HashedValues", t => t.Password_Id)
                .Index(t => t.Password_Id);
            
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
            
            CreateTable(
                "dbo.AuthorizationCodeScopes",
                c => new
                    {
                        AuthorizationCode_Code = c.String(nullable: false, maxLength: 128),
                        Scope_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.AuthorizationCode_Code, t.Scope_Id })
                .ForeignKey("dbo.AuthorizationCodes", t => t.AuthorizationCode_Code, cascadeDelete: true)
                .ForeignKey("dbo.Scopes", t => t.Scope_Id, cascadeDelete: true)
                .Index(t => t.AuthorizationCode_Code)
                .Index(t => t.Scope_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccessTokens", "User_Id", "dbo.Users");
            DropForeignKey("dbo.AccessTokens", "TokenValue_Id", "dbo.HashedValues");
            DropForeignKey("dbo.Scopes", "AccessToken_Id", "dbo.AccessTokens");
            DropForeignKey("dbo.Users", "Password_Id", "dbo.HashedValues");
            DropForeignKey("dbo.AuthorizationCodes", "User_Id", "dbo.Users");
            DropForeignKey("dbo.AuthorizationCodeScopes", "Scope_Id", "dbo.Scopes");
            DropForeignKey("dbo.AuthorizationCodeScopes", "AuthorizationCode_Code", "dbo.AuthorizationCodes");
            DropForeignKey("dbo.AuthorizationCodes", "Client_Name", "dbo.Clients");
            DropForeignKey("dbo.AccessTokens", "Client_Name", "dbo.Clients");
            DropForeignKey("dbo.RedirectUris", "Client_Name", "dbo.Clients");
            DropIndex("dbo.AuthorizationCodeScopes", new[] { "Scope_Id" });
            DropIndex("dbo.AuthorizationCodeScopes", new[] { "AuthorizationCode_Code" });
            DropIndex("dbo.Users", new[] { "Password_Id" });
            DropIndex("dbo.AuthorizationCodes", new[] { "User_Id" });
            DropIndex("dbo.AuthorizationCodes", new[] { "Client_Name" });
            DropIndex("dbo.Scopes", new[] { "AccessToken_Id" });
            DropIndex("dbo.RedirectUris", new[] { "Client_Name" });
            DropIndex("dbo.AccessTokens", new[] { "User_Id" });
            DropIndex("dbo.AccessTokens", new[] { "TokenValue_Id" });
            DropIndex("dbo.AccessTokens", new[] { "Client_Name" });
            DropTable("dbo.AuthorizationCodeScopes");
            DropTable("dbo.HashedValues");
            DropTable("dbo.Users");
            DropTable("dbo.AuthorizationCodes");
            DropTable("dbo.Scopes");
            DropTable("dbo.RedirectUris");
            DropTable("dbo.Clients");
            DropTable("dbo.AccessTokens");
        }
    }
}
