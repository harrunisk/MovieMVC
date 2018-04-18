namespace movieMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class news_model_update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "NewsDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "NewsDate");
        }
    }
}
