namespace Lexicon_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addmorecoursesmodulesactivitiesalsoaddrequiredattributeonactivitiesandnotification : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notifications", "Sender_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "Sender_Id" });
            AlterColumn("dbo.Activities", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Courses", "CourseCode", c => c.String(nullable: false));
            AlterColumn("dbo.Courses", "CourseName", c => c.String(nullable: false));
            AlterColumn("dbo.Notifications", "Subject", c => c.String(nullable: false));
            AlterColumn("dbo.Notifications", "Sender_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Notifications", "Sender_Id");
            AddForeignKey("dbo.Notifications", "Sender_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "Sender_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "Sender_Id" });
            AlterColumn("dbo.Notifications", "Sender_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Notifications", "Subject", c => c.String());
            AlterColumn("dbo.Courses", "CourseName", c => c.String());
            AlterColumn("dbo.Courses", "CourseCode", c => c.String());
            AlterColumn("dbo.Activities", "Name", c => c.String());
            CreateIndex("dbo.Notifications", "Sender_Id");
            AddForeignKey("dbo.Notifications", "Sender_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
