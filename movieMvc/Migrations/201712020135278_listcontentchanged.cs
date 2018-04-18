namespace movieMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class listcontentchanged : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Movies", "ListContent_Id", "dbo.ListContents");
            DropForeignKey("dbo.ListContents", "Movie_MovieID", "dbo.Movies");
            DropIndex("dbo.Movies", new[] { "ListContent_Id" });
            DropIndex("dbo.ListContents", new[] { "Movie_MovieID" });
            DropColumn("dbo.ListContents", "MovieID");
            RenameColumn(table: "dbo.ListContents", name: "Movie_MovieID", newName: "MovieID");
            AlterColumn("dbo.ListContents", "MovieID", c => c.Int(nullable: false));
            CreateIndex("dbo.ListContents", "MovieID");
            AddForeignKey("dbo.ListContents", "MovieID", "dbo.Movies", "MovieID", cascadeDelete: true);
            DropColumn("dbo.Movies", "ListContent_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "ListContent_Id", c => c.Int());
            DropForeignKey("dbo.ListContents", "MovieID", "dbo.Movies");
            DropIndex("dbo.ListContents", new[] { "MovieID" });
            AlterColumn("dbo.ListContents", "MovieID", c => c.Int());
            RenameColumn(table: "dbo.ListContents", name: "MovieID", newName: "Movie_MovieID");
            AddColumn("dbo.ListContents", "MovieID", c => c.Int(nullable: false));
            CreateIndex("dbo.ListContents", "Movie_MovieID");
            CreateIndex("dbo.Movies", "ListContent_Id");
            AddForeignKey("dbo.ListContents", "Movie_MovieID", "dbo.Movies", "MovieID");
            AddForeignKey("dbo.Movies", "ListContent_Id", "dbo.ListContents", "Id");
        }
    }
}
