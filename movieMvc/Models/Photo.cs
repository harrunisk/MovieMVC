using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace movieMvc.Models

    {
    public class Photo
    {

        //[DatabaseGenerated(DatabaseGenerated.Option.None)]
        //üstteki kot Pk olsun ama auto increment olmasın 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PhotoID { get; set; }
        public byte[] PhotoContent { get; set; }

        public virtual ICollection<MoviePhotos> MoviePhoto { get; set; }





        //fotoğrafın filmlerini tutacak aslında buna gerek yok fotoğrafın filmi olmaz 


    }
}