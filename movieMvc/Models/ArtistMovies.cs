using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace movieMvc.Models
{
    public class ArtistMovies
    {

        //burada izin veridi userMovie de izin vermedi
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int MovieID { get; set; }
        public virtual Movie Movie { get; set; }

        
        public int ArtistID { get; set; }
        public virtual Artist Artist     { get; set; }

    }
}