namespace EgyGobs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updates_jobs_model1_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "JobImage", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "JobImage");
        }
    }
}
