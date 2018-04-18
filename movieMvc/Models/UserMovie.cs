using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace movieMvc.Models
{
    public class UserMovie
    {



        //2 tane primary key yapısına izin vermiyor codefirst yapısı

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

              
        public int MovieID { get; set; }        
        public virtual Movie Movie { get; set; }

        public int Score { get; set; }
        public string Comment { get; set; }
        public bool Favorite { get; set; }

    }
}