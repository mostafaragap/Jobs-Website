namespace EgyGobs.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Country_type : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "usertype", c => c.String());
            AddColumn("dbo.AspNetUsers", "country", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "country");
            DropColumn("dbo.AspNetUsers", "usertype");
        }
    }
}
