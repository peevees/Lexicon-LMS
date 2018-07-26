namespace Lexicon_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sendernotrequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Notifications", "Sender_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "Sender_Id" });
            AlterColumn("dbo.Notifications", "Sender_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Notifications", "Sender_Id");
            AddForeignKey("dbo.Notifications", "Sender_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "Sender_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Notifications", new[] { "Sender_Id" });
            AlterColumn("dbo.Notifications", "Sender_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Notifications", "Sender_Id");
            AddForeignKey("dbo.Notifications", "Sender_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
