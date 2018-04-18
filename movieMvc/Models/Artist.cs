using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using movieMvc.Resources;

namespace movieMvc.Models
{
    public class Artist
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArtistID { get; set; }
        [Display(ResourceType = typeof(Home), Name = "ArtistName")]
        public string ArtistName { get; set; }
        [Display(ResourceType = typeof(Home), Name = "ArtistBirth")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime ArtistBirth { get; set; }
        [Display(ResourceType = typeof(Home), Name = "ArtistPlaceOfBirth")]
        public string ArtistPlaceOfBirth { get; set; }
        [Display(ResourceType = typeof(Home), Name = "ArtistBiography")]
        public string ArtistBiography { get; set; }
        [Display(ResourceType = typeof(Home), Name = "ArtistGeneralScore")]
        public float ArtisGeneralScore { get; set; }


        public virtual ICollection<ArtistPhoto> ArtistPhoto { get; set; }
        public virtual ICollection<ArtistMovies> ArtistMovie { get; set; }

    }
}