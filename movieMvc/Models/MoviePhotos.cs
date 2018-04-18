using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace movieMvc.Models
{
    public class MoviePhotos
    {

        //Bunlar foreign key oluyorlar

        //burada izin veridi userMovie de izin vermedi
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Movie")]
        public int MovieID { get; set; }       
        public virtual Movie Movie { get; set; }

       
        public int PhotoID { get; set; }        
        public virtual Photo Photo { get; set; }

        //aşağıdaki satırlar bunları FK yapıyor

    }
}