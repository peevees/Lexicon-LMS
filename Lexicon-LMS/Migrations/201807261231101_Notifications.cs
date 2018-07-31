namespace Lexicon_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Notifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "DateSent", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Forename", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Surname", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Surname", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Forename", c => c.String());
            DropColumn("dbo.Notifications", "DateSent");
        }
    }
}
