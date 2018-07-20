namespace Lexicon_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modulesactivities : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Activities", "Module_ID", "dbo.Modules");
            DropIndex("dbo.Activities", new[] { "Module_ID" });
            RenameColumn(table: "dbo.Activities", name: "Module_ID", newName: "ModuleID");
            AlterColumn("dbo.Activities", "ModuleID", c => c.Int(nullable: false));
            CreateIndex("dbo.Activities", "ModuleID");
            AddForeignKey("dbo.Activities", "ModuleID", "dbo.Modules", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Activities", "ModuleID", "dbo.Modules");
            DropIndex("dbo.Activities", new[] { "ModuleID" });
            AlterColumn("dbo.Activities", "ModuleID", c => c.Int());
            RenameColumn(table: "dbo.Activities", name: "ModuleID", newName: "Module_ID");
            CreateIndex("dbo.Activities", "Module_ID");
            AddForeignKey("dbo.Activities", "Module_ID", "dbo.Modules", "ID");
        }
    }
}
