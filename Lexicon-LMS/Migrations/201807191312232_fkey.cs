namespace Lexicon_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fkey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Modules", "Course_ID", "dbo.Courses");
            DropIndex("dbo.Modules", new[] { "Course_ID" });
            RenameColumn(table: "dbo.Modules", name: "Course_ID", newName: "CourseID");
            AlterColumn("dbo.Modules", "CourseID", c => c.Int(nullable: false));
            CreateIndex("dbo.Modules", "CourseID");
            AddForeignKey("dbo.Modules", "CourseID", "dbo.Courses", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Modules", "CourseID", "dbo.Courses");
            DropIndex("dbo.Modules", new[] { "CourseID" });
            AlterColumn("dbo.Modules", "CourseID", c => c.Int());
            RenameColumn(table: "dbo.Modules", name: "CourseID", newName: "Course_ID");
            CreateIndex("dbo.Modules", "Course_ID");
            AddForeignKey("dbo.Modules", "Course_ID", "dbo.Courses", "ID");
        }
    }
}
