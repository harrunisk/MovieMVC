using movieMvc.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace movieMvc.Models
{
    public class Movie
    {
        //veri tipi properties
        //class name ile bitişik ID varsa yada ID ise pk olarak ve increment olarak tanımlıyor 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovieID { get; set; }

        [Display(ResourceType = typeof(Home), Name = "MovieName")]
        public string MovieName { get; set; }

        [Display(ResourceType = typeof(Home), Name = "MovieReleaseDate")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime MovieReleaseDate { get; set; }

        [Display(ResourceType = typeof(Home), Name = "MovieGeneralScore")]
        public Double MovieGeneralScore { get; set; }

        [Display(ResourceType = typeof(Home), Name = "MovieGenre")]
        public string MovieGenre { get; set; }

        [Display(ResourceType = typeof(Home), Name = "MovieDirector")]
        public string MovieDirector { get; set; }

        [Display(ResourceType = typeof(Home), Name = "MovieTrailer")]
        public string MovieTrailer { get; set; }

        [Display(ResourceType = typeof(Home), Name = "MovieInformation")]
        public string MovieInformation { get; set; }

        [Display(ResourceType = typeof(Home), Name = "MovieDuration")]
        public int MovieDuration { get; set; }

        //bir filmin birden çok fotosu olabilir bunları burada tutmak lazım
        //navigation properties anlamına geliyor
        //Icollection interfacesinde tutuyoruz
        //virtual olmasının sebebi:lazy loading özelliği
        //Icollection düzeltme ekleme silme özellikleri sağlar 
        


        //bir filmin bir sürü fotosu olabilir bu yüzden bire çok ilişki ICOLLECTION
        public virtual ICollection<MoviePhotos> MoviePhoto { get; set; }

        //bir movie birden çok kişinin favori filmi olabilir o yüzden burası 
        public virtual ICollection<UserMovie> UserMovie { get; set; }

        public virtual ICollection<ArtistMovies> ArtistMovie { get; set; }





    }
}