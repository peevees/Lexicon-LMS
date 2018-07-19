namespace Lexicon_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Deadline = c.DateTime(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Module_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Modules", t => t.Module_ID)
                .Index(t => t.Module_ID);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Filepath = c.String(),
                        Activity_ID = c.Int(),
                        Course_ID = c.Int(),
                        Module_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Activities", t => t.Activity_ID)
                .ForeignKey("dbo.Courses", t => t.Course_ID)
                .ForeignKey("dbo.Modules", t => t.Module_ID)
                .Index(t => t.Activity_ID)
                .Index(t => t.Course_ID)
                .Index(t => t.Module_ID);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CourseCode = c.String(),
                        CourseName = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Teacher_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Teacher_Id)
                .Index(t => t.Teacher_Id);
            
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Course_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.Course_ID)
                .Index(t => t.Course_ID);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Body = c.String(),
                        Read = c.Boolean(nullable: false),
                        Attachment_ID = c.Int(),
                        Sender_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Documents", t => t.Attachment_ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Sender_Id)
                .Index(t => t.Attachment_ID)
                .Index(t => t.Sender_Id);
            
            AddColumn("dbo.AspNetUsers", "Course_ID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Course_ID");
            AddForeignKey("dbo.AspNetUsers", "Course_ID", "dbo.Courses", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "Sender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "Attachment_ID", "dbo.Documents");
            DropForeignKey("dbo.Courses", "Teacher_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.Documents", "Module_ID", "dbo.Modules");
            DropForeignKey("dbo.Activities", "Module_ID", "dbo.Modules");
            DropForeignKey("dbo.Modules", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.Documents", "Course_ID", "dbo.Courses");
            DropForeignKey("dbo.Documents", "Activity_ID", "dbo.Activities");
            DropIndex("dbo.Notifications", new[] { "Sender_Id" });
            DropIndex("dbo.Notifications", new[] { "Attachment_ID" });
            DropIndex("dbo.AspNetUsers", new[] { "Course_ID" });
            DropIndex("dbo.Modules", new[] { "Course_ID" });
            DropIndex("dbo.Courses", new[] { "Teacher_Id" });
            DropIndex("dbo.Documents", new[] { "Module_ID" });
            DropIndex("dbo.Documents", new[] { "Course_ID" });
            DropIndex("dbo.Documents", new[] { "Activity_ID" });
            DropIndex("dbo.Activities", new[] { "Module_ID" });
            DropColumn("dbo.AspNetUsers", "Course_ID");
            DropTable("dbo.Notifications");
            DropTable("dbo.Modules");
            DropTable("dbo.Courses");
            DropTable("dbo.Documents");
            DropTable("dbo.Activities");
        }
    }
}
