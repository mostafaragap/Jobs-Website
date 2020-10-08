using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

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
        [AllowHtml]
        public string JobContent { get; set; }
       
       [DisplayName("صورة الوظيفة")]
        public byte[] JobImage { get; set; }
        
        [DisplayName("نوع الوظيفة")]
        public int CategoryId { get; set; }
        public string UserId { get; set; }

        public virtual Category category { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}