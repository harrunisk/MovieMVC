using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace movieMvc.Models
{
    public class ArtistPhoto
    {
        //burada izin veridi userMovie de izin vermedi
        //column 1 ve 2
        //uygulama karar veremiyor sıralamaya o yüzden biz belirliyoruz
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ArtistID { get; set; }
        public virtual Artist Artist { get; set; }

        
        public int PhotoID { get; set; }
        public virtual Photo Photo { get; set; }

    }
}