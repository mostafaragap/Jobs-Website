using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication1.Models;
using Microsoft.AspNet.Identity.Owin;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var list = db.Categories.ToList();
            return View(list);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public FileContentResult UserPhotos( int id)
        {
            
                //String jobid = User.Identity.GetUserId();

                if (id == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);

                    return File(imageData, "image/png");

                }
                // to get the job details to load user Image
                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

                var userImage = bdUsers.Jobs.Where(x => x.Id == id).FirstOrDefault();

                return new FileContentResult(userImage.JobImage, "image/jpeg");
            
             
        }

    }
}