using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EgyGobs.Models
{
    public class Category
    {
        public int Id { get; set; }
     [Required]
     [DisplayName("نوع الوظيفة")]
      public string CategoryName { get; set; }
       [Required]
        [DisplayName("وصف النوع")]
        public string description { get; set; }

        public virtual ICollection<Job> jobs { get; set; }
    }
}