namespace Lexicon_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cantlogin : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notifications", "Sender_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "Sender_Id" });
            AddColumn("dbo.AspNetUsers", "Notification_ID", c => c.Int());
            AddColumn("dbo.Notifications", "RecipientID", c => c.String(nullable: false));
            AddColumn("dbo.Notifications", "DateSent", c => c.DateTime(nullable: false));
            AddColumn("dbo.Notifications", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.Notifications", "Sender_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "Notification_ID");
            CreateIndex("dbo.Notifications", "Sender_Id");
            CreateIndex("dbo.Notifications", "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetUsers", "Notification_ID", "dbo.Notifications", "ID");
            AddForeignKey("dbo.Notifications", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Notifications", "Sender_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "Sender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Notifications", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Notification_ID", "dbo.Notifications");
            DropIndex("dbo.Notifications", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Notifications", new[] { "Sender_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "Notification_ID" });
            AlterColumn("dbo.Notifications", "Sender_Id", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Notifications", "ApplicationUser_Id");
            DropColumn("dbo.Notifications", "DateSent");
            DropColumn("dbo.Notifications", "RecipientID");
            DropColumn("dbo.AspNetUsers", "Notification_ID");
            CreateIndex("dbo.Notifications", "Sender_Id");
            AddForeignKey("dbo.Notifications", "Sender_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
