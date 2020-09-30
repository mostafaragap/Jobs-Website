using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EgyGobs.Models
{
    public class Job
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("اسم الوظيفة")]
        public string JobTitle { get; set; }
        [Required]
        [DisplayName("وصف الوظيفة")]
        public string JobContent { get; set; }
       
       [DisplayName("صورة الوظيفة")]
        public byte[] JobImage { get; set; }
        
        [DisplayName("نوع الوظيفة")]
        public int CategoryId { get; set; }

        public virtual Category category { get; set; }
    }
}