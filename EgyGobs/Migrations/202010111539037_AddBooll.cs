namespace EgyGobs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBooll : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplyForJobs", "IsConfirmed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplyForJobs", "IsConfirmed");
        }
    }
}
