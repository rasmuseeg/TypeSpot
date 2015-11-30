namespace Typespot.Migrations
{
  using System;
  using System.Data.Entity.Migrations;

  public partial class IdentityFix : DbMigration
  {
    public override void Up()
    {
      // These keys does not exist?
      DropForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers");
      DropForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers");
      DropForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers");
      DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
      // Reordered these to changes above
      RenameTable(name: "dbo.ApplicationUsers", newName: "Users");
      RenameTable(name: "dbo.IdentityUserClaims", newName: "UserClaims");
      RenameTable(name: "dbo.IdentityUserLogins", newName: "UserLogins");
      RenameTable(name: "dbo.IdentityUserRoles", newName: "UserRoles");
      RenameTable(name: "dbo.IdentityRoles", newName: "Roles");

      DropIndex("dbo.UserClaims", new[] { "ApplicationUser_Id" });
      DropIndex("dbo.UserLogins", new[] { "ApplicationUser_Id" });
      DropIndex("dbo.UserRoles", new[] { "ApplicationUser_Id" });
      DropIndex("dbo.UserRoles", new[] { "IdentityRole_Id" });
      // remove unused columns
      DropColumn("dbo.UserClaims", "ApplicationUser_Id");
      DropColumn("dbo.UserLogins", "ApplicationUser_Id");
      DropColumn("dbo.UserRoles", "ApplicationUser_Id");
      DropColumn("dbo.UserRoles", "IdentityRole_Id");
      //RenameColumn(table: "dbo.UserClaims", name: "ApplicationUser_Id", newName: "UserId");
      //RenameColumn(table: "dbo.UserLogins", name: "ApplicationUser_Id", newName: "UserId");
      //RenameColumn(table: "dbo.UserRoles", name: "ApplicationUser_Id", newName: "UserId");
      //RenameColumn(table: "dbo.UserRoles", name: "IdentityRole_Id", newName: "RoleId");
      //DropPrimaryKey("dbo.UserLogins");
      //DropPrimaryKey("dbo.UserRoles");
      AlterColumn("dbo.Users", "Email", c => c.String(maxLength: 256));
      AlterColumn("dbo.Users", "UserName", c => c.String(nullable: false, maxLength: 256));
      AlterColumn("dbo.UserClaims", "UserId", c => c.String(nullable: false, maxLength: 128));
      AlterColumn("dbo.UserClaims", "UserId", c => c.String(nullable: false, maxLength: 128));
      AlterColumn("dbo.UserLogins", "LoginProvider", c => c.String(nullable: false, maxLength: 128));
      AlterColumn("dbo.UserLogins", "ProviderKey", c => c.String(nullable: false, maxLength: 128));
      AlterColumn("dbo.UserLogins", "UserId", c => c.String(nullable: false, maxLength: 128));
      AlterColumn("dbo.UserRoles", "UserId", c => c.String(nullable: false, maxLength: 128));
      AlterColumn("dbo.UserRoles", "RoleId", c => c.String(nullable: false, maxLength: 128));
      AlterColumn("dbo.Roles", "Name", c => c.String(nullable: false, maxLength: 256));
      //AddPrimaryKey("dbo.UserLogins", new[] { "LoginProvider", "ProviderKey", "UserId" });
      //AddPrimaryKey("dbo.UserRoles", new[] { "UserId", "RoleId" });
      CreateIndex("dbo.Users", "UserName", unique: true, name: "UserNameIndex");
      CreateIndex("dbo.UserClaims", "UserId");
      CreateIndex("dbo.UserLogins", "UserId");
      CreateIndex("dbo.UserRoles", "UserId");
      CreateIndex("dbo.UserRoles", "RoleId");
      CreateIndex("dbo.Roles", "Name", unique: true, name: "RoleNameIndex");
      AddForeignKey("dbo.UserClaims", "UserId", "dbo.Users", "Id", cascadeDelete: true);
      AddForeignKey("dbo.UserLogins", "UserId", "dbo.Users", "Id", cascadeDelete: true);
      //AddForeignKey("dbo.UserRoles", "UserId", "dbo.Users", "Id", cascadeDelete: true);
      AddForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles", "Id", cascadeDelete: true);
    }

    public override void Down()
    {
      DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
      DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
      DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
      DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
      DropIndex("dbo.Roles", "RoleNameIndex");
      DropIndex("dbo.UserRoles", new[] { "RoleId" });
      DropIndex("dbo.UserRoles", new[] { "UserId" });
      DropIndex("dbo.UserLogins", new[] { "UserId" });
      DropIndex("dbo.UserClaims", new[] { "UserId" });
      DropIndex("dbo.Users", "UserNameIndex");
      //DropPrimaryKey("dbo.UserRoles");
      //DropPrimaryKey("dbo.UserLogins");
      AlterColumn("dbo.Roles", "Name", c => c.String());
      AlterColumn("dbo.UserRoles", "RoleId", c => c.String(maxLength: 128));
      AlterColumn("dbo.UserRoles", "UserId", c => c.String(maxLength: 128));
      AlterColumn("dbo.UserLogins", "UserId", c => c.String(maxLength: 128));
      AlterColumn("dbo.UserLogins", "ProviderKey", c => c.String());
      AlterColumn("dbo.UserLogins", "LoginProvider", c => c.String());
      AlterColumn("dbo.UserClaims", "UserId", c => c.String(maxLength: 128));
      AlterColumn("dbo.UserClaims", "UserId", c => c.String());
      AlterColumn("dbo.Users", "UserName", c => c.String());
      AlterColumn("dbo.Users", "Email", c => c.String());
      AddPrimaryKey("dbo.UserRoles", new[] { "RoleId", "UserId" });
      AddPrimaryKey("dbo.UserLogins", "UserId");
      RenameColumn(table: "dbo.UserRoles", name: "RoleId", newName: "IdentityRole_Id");
      RenameColumn(table: "dbo.UserRoles", name: "UserId", newName: "ApplicationUser_Id");
      RenameColumn(table: "dbo.UserLogins", name: "UserId", newName: "ApplicationUser_Id");
      RenameColumn(table: "dbo.UserClaims", name: "UserId", newName: "ApplicationUser_Id");
      //AddColumn("dbo.UserRoles", "RoleId", c => c.String(nullable: false, maxLength: 128));
      //AddColumn("dbo.UserRoles", "UserId", c => c.String(nullable: false, maxLength: 128));
      //AddColumn("dbo.UserLogins", "UserId", c => c.String(nullable: false, maxLength: 128));
      //AddColumn("dbo.UserClaims", "UserId", c => c.String());
      CreateIndex("dbo.UserRoles", "IdentityRole_Id");
      CreateIndex("dbo.UserRoles", "ApplicationUser_Id");
      CreateIndex("dbo.UserLogins", "ApplicationUser_Id");
      CreateIndex("dbo.UserClaims", "ApplicationUser_Id");
      AddForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles", "Id");
      AddForeignKey("dbo.IdentityUserRoles", "ApplicationUser_Id", "dbo.ApplicationUsers", "Id");
      AddForeignKey("dbo.IdentityUserLogins", "ApplicationUser_Id", "dbo.ApplicationUsers", "Id");
      AddForeignKey("dbo.IdentityUserClaims", "ApplicationUser_Id", "dbo.ApplicationUsers", "Id");
      RenameTable(name: "dbo.Roles", newName: "IdentityRoles");
      RenameTable(name: "dbo.UserRoles", newName: "IdentityUserRoles");
      RenameTable(name: "dbo.UserLogins", newName: "IdentityUserLogins");
      RenameTable(name: "dbo.UserClaims", newName: "IdentityUserClaims");
      RenameTable(name: "dbo.Users", newName: "ApplicationUsers");
    }
  }
}
