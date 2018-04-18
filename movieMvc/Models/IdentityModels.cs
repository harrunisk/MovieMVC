using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace movieMvc.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public bool? Sex { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime? Birth { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]

        public DateTime? RegisterDate { get; set; }
        public byte[] UserPhoto { get; set; }

        public virtual UserMovie UserMovie { get; set; }
        public virtual UserArtist UserArtist { get; set; }

        public virtual ICollection<UserMovie> UserMovies { get; set; }
        public virtual ICollection<UserArtist> UserArtists { get; set; }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

      

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection2", throwIfV1Schema: false)

        {
            
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
        public DbSet<Movie> MovieFunc { get; set; }
        public DbSet<Photo> PhotoFunc { get; set; }
        public DbSet<MoviePhotos> MoviePhotosFunc { get; set; }
        public DbSet<UserMovie> UserMovie { get; set; }
        public DbSet<UserArtist> UserArtist { get; set; }
        public DbSet<Artist> Artist { get; set; }
        public DbSet<ArtistMovies> ArtistMovies { get; set; }
        public DbSet<ArtistPhoto> ArtistPhoto { get; set; }
        public DbSet<List> List { get; set; }
        public DbSet<ListContent> ListContent { get; set; }
        public DbSet<ListPhoto> ListPhoto { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsPhoto> NewsPhoto { get; set; }
        
    }
}