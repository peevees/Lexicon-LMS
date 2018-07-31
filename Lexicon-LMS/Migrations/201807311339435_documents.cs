namespace Lexicon_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class documents : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Documents", name: "Activity_ID", newName: "ActivityID");
            RenameColumn(table: "dbo.Documents", name: "Course_ID", newName: "CourseID");
            RenameColumn(table: "dbo.Documents", name: "Module_ID", newName: "ModuleID");
            RenameIndex(table: "dbo.Documents", name: "IX_Course_ID", newName: "IX_CourseID");
            RenameIndex(table: "dbo.Documents", name: "IX_Module_ID", newName: "IX_ModuleID");
            RenameIndex(table: "dbo.Documents", name: "IX_Activity_ID", newName: "IX_ActivityID");
            AddColumn("dbo.Documents", "FileName", c => c.String());
            AddColumn("dbo.Documents", "DisplayName", c => c.String());
            AddColumn("dbo.Documents", "UploadDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Documents", "User_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Modules", "CourseCode", c => c.String());
            CreateIndex("dbo.Documents", "User_Id");
            AddForeignKey("dbo.Documents", "User_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Documents", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "Name", c => c.String());
            DropForeignKey("dbo.Documents", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Documents", new[] { "User_Id" });
            AlterColumn("dbo.Modules", "CourseCode", c => c.Int(nullable: false));
            DropColumn("dbo.Documents", "User_Id");
            DropColumn("dbo.Documents", "UploadDate");
            DropColumn("dbo.Documents", "DisplayName");
            DropColumn("dbo.Documents", "FileName");
            RenameIndex(table: "dbo.Documents", name: "IX_ActivityID", newName: "IX_Activity_ID");
            RenameIndex(table: "dbo.Documents", name: "IX_ModuleID", newName: "IX_Module_ID");
            RenameIndex(table: "dbo.Documents", name: "IX_CourseID", newName: "IX_Course_ID");
            RenameColumn(table: "dbo.Documents", name: "ModuleID", newName: "Module_ID");
            RenameColumn(table: "dbo.Documents", name: "CourseID", newName: "Course_ID");
            RenameColumn(table: "dbo.Documents", name: "ActivityID", newName: "Activity_ID");
        }
    }
}
