namespace movieMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ilk : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        ArtistID = c.Int(nullable: false, identity: true),
                        ArtistName = c.String(),
                        ArtistBirth = c.DateTime(nullable: false),
                        ArtistPlaceOfBirth = c.String(),
                        ArtistBiography = c.String(),
                        ArtisGeneralScore = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ArtistID);
            
            CreateTable(
                "dbo.ArtistMovies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MovieID = c.Int(nullable: false),
                        ArtistID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Artists", t => t.ArtistID, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.MovieID, cascadeDelete: true)
                .Index(t => t.MovieID)
                .Index(t => t.ArtistID);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        MovieID = c.Int(nullable: false, identity: true),
                        MovieName = c.String(),
                        MovieReleaseDate = c.DateTime(nullable: false),
                        MovieGeneralScore = c.Double(nullable: false),
                        MovieGenre = c.String(),
                        MovieDirector = c.String(),
                        MovieTrailer = c.String(),
                        MovieInformation = c.String(),
                        MovieDuration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MovieID);
            
            CreateTable(
                "dbo.MoviePhotos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MovieID = c.Int(nullable: false),
                        PhotoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Movies", t => t.MovieID, cascadeDelete: true)
                .ForeignKey("dbo.Photos", t => t.PhotoID, cascadeDelete: true)
                .Index(t => t.MovieID)
                .Index(t => t.PhotoID);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoID = c.Int(nullable: false, identity: true),
                        PhotoContent = c.Binary(),
                    })
                .PrimaryKey(t => t.PhotoID);
            
            CreateTable(
                "dbo.UserMovies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        MovieID = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                        Comment = c.String(),
                        Favorite = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Movies", t => t.MovieID, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.MovieID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Sex = c.Boolean(),
                        Birth = c.DateTime(),
                        RegisterDate = c.DateTime(),
                        UserPhoto = c.Binary(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        UserArtist_Id = c.Int(),
                        UserMovie_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserArtists", t => t.UserArtist_Id)
                .ForeignKey("dbo.UserMovies", t => t.UserMovie_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.UserArtist_Id)
                .Index(t => t.UserMovie_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserArtists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ArtistID = c.Int(nullable: false),
                        Score = c.Int(nullable: false),
                        Comment = c.String(),
                        Favorite = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Artists", t => t.ArtistID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.UserId)
                .Index(t => t.ArtistID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ArtistPhotoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArtistID = c.Int(nullable: false),
                        PhotoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Artists", t => t.ArtistID, cascadeDelete: true)
                .ForeignKey("dbo.Photos", t => t.PhotoID, cascadeDelete: true)
                .Index(t => t.ArtistID)
                .Index(t => t.PhotoID);
            
            CreateTable(
                "dbo.Lists",
                c => new
                    {
                        ListID = c.Int(nullable: false, identity: true),
                        ListTitle = c.String(),
                    })
                .PrimaryKey(t => t.ListID);
            
            CreateTable(
                "dbo.ListPhotoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ListID = c.Int(nullable: false),
                        PhotoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lists", t => t.ListID, cascadeDelete: true)
                .ForeignKey("dbo.Photos", t => t.PhotoID, cascadeDelete: true)
                .Index(t => t.ListID)
                .Index(t => t.PhotoID);
            
            CreateTable(
                "dbo.ListContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MovieID = c.Int(nullable: false),
                        ListID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lists", t => t.ListID, cascadeDelete: true)
                .ForeignKey("dbo.Movies", t => t.MovieID, cascadeDelete: true)
                .Index(t => t.MovieID)
                .Index(t => t.ListID);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        NewsID = c.Int(nullable: false, identity: true),
                        NewsContent = c.String(),
                        NewsTitle = c.String(),
                    })
                .PrimaryKey(t => t.NewsID);
            
            CreateTable(
                "dbo.NewsPhotoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NewsID = c.Int(nullable: false),
                        PhotoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.News", t => t.NewsID, cascadeDelete: true)
                .ForeignKey("dbo.Photos", t => t.PhotoID, cascadeDelete: true)
                .Index(t => t.NewsID)
                .Index(t => t.PhotoID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.NewsPhotoes", "PhotoID", "dbo.Photos");
            DropForeignKey("dbo.NewsPhotoes", "NewsID", "dbo.News");
            DropForeignKey("dbo.ListContents", "MovieID", "dbo.Movies");
            DropForeignKey("dbo.ListContents", "ListID", "dbo.Lists");
            DropForeignKey("dbo.ListPhotoes", "PhotoID", "dbo.Photos");
            DropForeignKey("dbo.ListPhotoes", "ListID", "dbo.Lists");
            DropForeignKey("dbo.ArtistPhotoes", "PhotoID", "dbo.Photos");
            DropForeignKey("dbo.ArtistPhotoes", "ArtistID", "dbo.Artists");
            DropForeignKey("dbo.ArtistMovies", "MovieID", "dbo.Movies");
            DropForeignKey("dbo.UserMovies", "MovieID", "dbo.Movies");
            DropForeignKey("dbo.UserMovies", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserMovies", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "UserMovie_Id", "dbo.UserMovies");
            DropForeignKey("dbo.UserArtists", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "UserArtist_Id", "dbo.UserArtists");
            DropForeignKey("dbo.UserArtists", "ArtistID", "dbo.Artists");
            DropForeignKey("dbo.UserArtists", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.MoviePhotos", "PhotoID", "dbo.Photos");
            DropForeignKey("dbo.MoviePhotos", "MovieID", "dbo.Movies");
            DropForeignKey("dbo.ArtistMovies", "ArtistID", "dbo.Artists");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.NewsPhotoes", new[] { "PhotoID" });
            DropIndex("dbo.NewsPhotoes", new[] { "NewsID" });
            DropIndex("dbo.ListContents", new[] { "ListID" });
            DropIndex("dbo.ListContents", new[] { "MovieID" });
            DropIndex("dbo.ListPhotoes", new[] { "PhotoID" });
            DropIndex("dbo.ListPhotoes", new[] { "ListID" });
            DropIndex("dbo.ArtistPhotoes", new[] { "PhotoID" });
            DropIndex("dbo.ArtistPhotoes", new[] { "ArtistID" });
            DropIndex("dbo.UserArtists", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.UserArtists", new[] { "ArtistID" });
            DropIndex("dbo.UserArtists", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "UserMovie_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "UserArtist_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.UserMovies", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.UserMovies", new[] { "MovieID" });
            DropIndex("dbo.UserMovies", new[] { "UserId" });
            DropIndex("dbo.MoviePhotos", new[] { "PhotoID" });
            DropIndex("dbo.MoviePhotos", new[] { "MovieID" });
            DropIndex("dbo.ArtistMovies", new[] { "ArtistID" });
            DropIndex("dbo.ArtistMovies", new[] { "MovieID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.NewsPhotoes");
            DropTable("dbo.News");
            DropTable("dbo.ListContents");
            DropTable("dbo.ListPhotoes");
            DropTable("dbo.Lists");
            DropTable("dbo.ArtistPhotoes");
            DropTable("dbo.UserArtists");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.UserMovies");
            DropTable("dbo.Photos");
            DropTable("dbo.MoviePhotos");
            DropTable("dbo.Movies");
            DropTable("dbo.ArtistMovies");
            DropTable("dbo.Artists");
        }
    }
}
