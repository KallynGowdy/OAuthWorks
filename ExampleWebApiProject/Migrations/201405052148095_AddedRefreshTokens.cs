namespace ExampleWebApiProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRefreshTokens : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Scopes", "AccessToken_Id", "dbo.AccessTokens");
            DropIndex("dbo.Scopes", new[] { "AccessToken_Id" });
            CreateTable(
                "dbo.RefreshTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Revoked = c.Boolean(nullable: false),
                        ExpirationDateUtc = c.DateTime(),
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
                "dbo.ScopeAccessTokens",
                c => new
                    {
                        Scope_Id = c.String(nullable: false, maxLength: 128),
                        AccessToken_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Scope_Id, t.AccessToken_Id })
                .ForeignKey("dbo.Scopes", t => t.Scope_Id, cascadeDelete: true)
                .ForeignKey("dbo.AccessTokens", t => t.AccessToken_Id, cascadeDelete: true)
                .Index(t => t.Scope_Id)
                .Index(t => t.AccessToken_Id);
            
            CreateTable(
                "dbo.RefreshTokenScopes",
                c => new
                    {
                        RefreshToken_Id = c.String(nullable: false, maxLength: 128),
                        Scope_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RefreshToken_Id, t.Scope_Id })
                .ForeignKey("dbo.RefreshTokens", t => t.RefreshToken_Id, cascadeDelete: true)
                .ForeignKey("dbo.Scopes", t => t.Scope_Id, cascadeDelete: true)
                .Index(t => t.RefreshToken_Id)
                .Index(t => t.Scope_Id);
            
            DropColumn("dbo.Scopes", "AccessToken_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Scopes", "AccessToken_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.RefreshTokens", "User_Id", "dbo.Users");
            DropForeignKey("dbo.RefreshTokens", "TokenValue_Id", "dbo.HashedValues");
            DropForeignKey("dbo.RefreshTokenScopes", "Scope_Id", "dbo.Scopes");
            DropForeignKey("dbo.RefreshTokenScopes", "RefreshToken_Id", "dbo.RefreshTokens");
            DropForeignKey("dbo.RefreshTokens", "Client_Name", "dbo.Clients");
            DropForeignKey("dbo.ScopeAccessTokens", "AccessToken_Id", "dbo.AccessTokens");
            DropForeignKey("dbo.ScopeAccessTokens", "Scope_Id", "dbo.Scopes");
            DropIndex("dbo.RefreshTokenScopes", new[] { "Scope_Id" });
            DropIndex("dbo.RefreshTokenScopes", new[] { "RefreshToken_Id" });
            DropIndex("dbo.ScopeAccessTokens", new[] { "AccessToken_Id" });
            DropIndex("dbo.ScopeAccessTokens", new[] { "Scope_Id" });
            DropIndex("dbo.RefreshTokens", new[] { "User_Id" });
            DropIndex("dbo.RefreshTokens", new[] { "TokenValue_Id" });
            DropIndex("dbo.RefreshTokens", new[] { "Client_Name" });
            DropTable("dbo.RefreshTokenScopes");
            DropTable("dbo.ScopeAccessTokens");
            DropTable("dbo.RefreshTokens");
            CreateIndex("dbo.Scopes", "AccessToken_Id");
            AddForeignKey("dbo.Scopes", "AccessToken_Id", "dbo.AccessTokens", "Id");
        }
    }
}
