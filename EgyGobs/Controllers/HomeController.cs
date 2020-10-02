using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebApplication1.Models;
using Microsoft.AspNet.Identity.Owin;
using EgyGobs.Models;
using System.Net;
using System.Data.Entity;

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
        public ActionResult Details (int job_Id)
        {
            var job = db.Jobs.Find(job_Id);
            if(job ==null)
            {
                return HttpNotFound();
            }
            Session["jobId"] = job_Id;
            return View(job);

        }
        [Authorize]
        public ActionResult Apply()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Apply( string message)
        {
            var user_id = User.Identity.GetUserId();
            var job_id = (int)Session["jobId"];
           

            var check = db.ApplyForJobs.Where(a=>a.jobId == job_id && a.userId ==user_id ).ToList();
   
            if(check.Count < 1)
            {
                ApplyForJob job = new ApplyForJob();
                job.userId = user_id;
                job.jobId = job_id;
                job.message = message;
                job.ApplayDate = DateTime.Now;
                db.ApplyForJobs.Add(job);
                db.SaveChanges();
                ViewBag.Result = " تمت عملية التقدم بنجاح";
                ViewBag.Color = "Green";
            }
            else
            {
                ViewBag.Result = "لقد سبق وتقدمت الى هذه الوظيفة";
                ViewBag.Color = null;

            }
             
            return View();
        }

        public ActionResult GetJobsByUser()
        {
            var UserId = User.Identity.GetUserId();
            var jobs = db.ApplyForJobs.Where(a => a.userId == UserId);
            return View(jobs.ToList()); 

        }
        [Authorize]
        public ActionResult DetailsJob(int id)
        {
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
    
            return View(job);

        }
        [Authorize]
        public ActionResult EditJob(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplyForJob job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
          
            return View(job);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditJob(ApplyForJob job)
        {
            if (ModelState.IsValid)
            {
                job.ApplayDate = DateTime.Now;
                db.Entry(job).State = EntityState.Modified;
               
                db.SaveChanges();
                return RedirectToAction("GetJobsByUser");
            }
           
            return View(job);
        }


        public ActionResult DeleteJob(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplyForJob job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("DeleteJob")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplyForJob job = db.ApplyForJobs.Find(id);
            db.ApplyForJobs.Remove(job);
            db.SaveChanges();
            return RedirectToAction("GetJobsByUser");
        }




    }
}