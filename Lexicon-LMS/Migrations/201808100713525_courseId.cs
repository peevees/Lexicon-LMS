namespace Lexicon_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class courseId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AspNetUsers", name: "UserCourse_ID", newName: "UserCourseId");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_UserCourse_ID", newName: "IX_UserCourseId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_UserCourseId", newName: "IX_UserCourse_ID");
            RenameColumn(table: "dbo.AspNetUsers", name: "UserCourseId", newName: "UserCourse_ID");
        }
    }
}
