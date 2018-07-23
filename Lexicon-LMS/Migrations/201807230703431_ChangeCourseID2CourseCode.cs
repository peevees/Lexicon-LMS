namespace Lexicon_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCourseID2CourseCode : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Modules", "CourseID", "dbo.Courses");
            DropIndex("dbo.Modules", new[] { "CourseID" });
            RenameColumn(table: "dbo.Modules", name: "CourseID", newName: "Course_ID");
            AddColumn("dbo.Modules", "CourseCode", c => c.Int(nullable: false));
            AlterColumn("dbo.Modules", "Course_ID", c => c.Int());
            CreateIndex("dbo.Modules", "Course_ID");
            AddForeignKey("dbo.Modules", "Course_ID", "dbo.Courses", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Modules", "Course_ID", "dbo.Courses");
            DropIndex("dbo.Modules", new[] { "Course_ID" });
            AlterColumn("dbo.Modules", "Course_ID", c => c.Int(nullable: false));
            DropColumn("dbo.Modules", "CourseCode");
            RenameColumn(table: "dbo.Modules", name: "Course_ID", newName: "CourseID");
            CreateIndex("dbo.Modules", "CourseID");
            AddForeignKey("dbo.Modules", "CourseID", "dbo.Courses", "ID", cascadeDelete: true);
        }
    }
}
