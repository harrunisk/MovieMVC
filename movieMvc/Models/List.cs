using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace movieMvc.Models
{
    public class List
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListID { get; set; }
        public string ListTitle { get; set; }
        public virtual ICollection<ListPhoto> ListPhotos {get;set;}
        public virtual ICollection<ListContent> ListContent { get; set; }

    }
}