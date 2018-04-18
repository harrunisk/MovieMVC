using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace movieMvc.Models
{
    public class ListContent
    {

        //burada izin veridi userMovie de izin vermedi
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Movie")]
        public int MovieID { get; set; }
        public virtual Movie Movie { get; set; }

        public int ListID { get; set; }
        public virtual List List { get; set; }
        IEnumerable<MultiSelectList> Movies { get; set; }

    }
}