namespace Lexicon_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recipientsnotrequiredreceipientidhoweverrequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notifications", "RecipientID", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notifications", "RecipientID", c => c.String());
        }
    }
}
