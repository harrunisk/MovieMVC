using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace movieMvc.Models
{
    public class NewsPhoto
    {


        //burada izin veridi userMovie de izin vermedi
        //column 1 ve 2
        //uygulama karar veremiyor sıralamaya o yüzden biz belirliyoruz
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        
        public int NewsID { get; set; }
        public virtual News News { get; set; }

        public int PhotoID { get; set; }
        public virtual Photo Photo { get; set; }
    }
}