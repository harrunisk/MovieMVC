using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace movieMvc.Models
{
    public class ListPhoto
    {

        //2 tane primary key yapısına izin vermiyor codefirst yapısı
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ListID { get; set; }
        public virtual List List { get; set; }

        
        public int PhotoID { get; set; }
        public virtual Photo Photo { get; set; }


    }
}