namespace movieMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class listContent : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ListContents", "MovieID", "dbo.Movies");
            DropIndex("dbo.ListContents", new[] { "MovieID" });
            AddColumn("dbo.Movies", "ListContent_Id", c => c.Int());
            AddColumn("dbo.ListContents", "Movie_MovieID", c => c.Int());
            CreateIndex("dbo.Movies", "ListContent_Id");
            CreateIndex("dbo.ListContents", "Movie_MovieID");
            AddForeignKey("dbo.Movies", "ListContent_Id", "dbo.ListContents", "Id");
            AddForeignKey("dbo.ListContents", "Movie_MovieID", "dbo.Movies", "MovieID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ListContents", "Movie_MovieID", "dbo.Movies");
            DropForeignKey("dbo.Movies", "ListContent_Id", "dbo.ListContents");
            DropIndex("dbo.ListContents", new[] { "Movie_MovieID" });
            DropIndex("dbo.Movies", new[] { "ListContent_Id" });
            DropColumn("dbo.ListContents", "Movie_MovieID");
            DropColumn("dbo.Movies", "ListContent_Id");
            CreateIndex("dbo.ListContents", "MovieID");
            AddForeignKey("dbo.ListContents", "MovieID", "dbo.Movies", "MovieID", cascadeDelete: true);
        }
    }
}
