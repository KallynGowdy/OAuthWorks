namespace ExampleWebApiProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedIdGenerationMode : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ScopeAuthorizationCodes", "AuthorizationCode_Code", "dbo.AuthorizationCodes");
            DropForeignKey("dbo.AuthorizationCodes", "User_Id", "dbo.Users");
            DropPrimaryKey("dbo.AuthorizationCodes");
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.AuthorizationCodes", "Code", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Users", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.AuthorizationCodes", "Code");
            AddPrimaryKey("dbo.Users", "Id");
            AddForeignKey("dbo.ScopeAuthorizationCodes", "AuthorizationCode_Code", "dbo.AuthorizationCodes", "Code", cascadeDelete: true);
            AddForeignKey("dbo.AuthorizationCodes", "User_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthorizationCodes", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ScopeAuthorizationCodes", "AuthorizationCode_Code", "dbo.AuthorizationCodes");
            DropPrimaryKey("dbo.Users");
            DropPrimaryKey("dbo.AuthorizationCodes");
            AlterColumn("dbo.Users", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AuthorizationCodes", "Code", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Users", "Id");
            AddPrimaryKey("dbo.AuthorizationCodes", "Code");
            AddForeignKey("dbo.AuthorizationCodes", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.ScopeAuthorizationCodes", "AuthorizationCode_Code", "dbo.AuthorizationCodes", "Code", cascadeDelete: true);
        }
    }
}
