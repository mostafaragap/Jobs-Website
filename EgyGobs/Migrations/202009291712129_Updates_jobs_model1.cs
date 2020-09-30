namespace EgyGobs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updates_jobs_model1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Jobs", "JobImage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Jobs", "JobImage", c => c.String());
        }
    }
}
