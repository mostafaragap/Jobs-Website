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
using System.Net.Mail;

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
        [HttpGet]
        public ActionResult Contact()
        {
           

            return View();
        }
        [HttpPost]
        public ActionResult Contact(ContactModel contact)
        {
            var mail = new MailMessage();
            var loginInfo = new NetworkCredential("devmostafa350@gmail.com", "MOstafa1234_");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add(new MailAddress("devmostafa350@gmail.com"));
            mail.Subject = contact.Subject;
            mail.IsBodyHtml = true; 
            string body = "اسم المرسل : " + contact.Name + "<br>" +
                " بريد المرسل : " + contact.Email + "<br>" +
                "عنوان الرسالة : " + contact.Subject + "<br>" +
                "نص الرسالة : <b>" + contact.Message + "</b>";
            mail.Body = body;

            var smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = loginInfo;
            smtpClient.Send(mail); 
            return RedirectToAction("Index");
        }
        public FileContentResult UserImage()
        {
            if (User.Identity.IsAuthenticated)
            {
                String userId = User.Identity.GetUserId();

                if (userId == null)
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
                // to get the user details to load user Image
                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

                var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();

                return new FileContentResult(userImage.UserImage, "image/jpeg");
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");

            }
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
                job.IsConfirmed = false;
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
                job.IsConfirmed = false;
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

        [Authorize]
       public ActionResult GetJobsByPublishers()
        {
            var userId = User.Identity.GetUserId();
            var Jobs = from app in db.ApplyForJobs
                       join job in db.Jobs
                       on app.jobId equals job.Id
                       where job.User.Id == userId && app.user.Id != null
                       select app ;
            var groubed = from j in Jobs
                          group j by j.job.JobTitle
                          into gr
                          select new JobsViewModel
                          {
                              JobTitle = gr.Key,
                              Items = gr
                          };

            if(Jobs.Count() < 1)
            {
                ViewBag.Res = "عفوا لم يتقدم اشخاصا بعد لوظائفك";
            }
            else
            {
                ViewBag.Res = "الاشخاص الذين تقدموا لوظائفك";
                return View(groubed.ToList());
            }
            return View(groubed.ToList());
        }

        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search( string searchName)
        {
            var result = db.Jobs.Where(a => a.JobTitle.Contains(searchName)
            || a.JobContent.Contains(searchName)
            || a.category.CategoryName.Contains(searchName)
            || a.category.description.Contains(searchName)); 
            if(result.Count() < 1 )
            {
                ViewBag.Result = "عذرا لا توجد نتائج .يرجى المحاولة مرة اخرى";
                return View(result.ToList());
            }else
            {
                ViewBag.Result = " ";
                return View(result.ToList());
            }
         
        }
        [HttpGet]
        public ActionResult AgreeForJob(int id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            ApplyForJob job = db.ApplyForJobs.Find(id);
            if(job ==null)
            {
                return HttpNotFound();

            }
            return View(job); 

        }

        [HttpPost, ActionName("AgreeForJob")]
        [ValidateAntiForgeryToken]
        public ActionResult AgreeConfirmed(int id)
        {
            ApplyForJob job = db.ApplyForJobs.Find(id);
            var UserId = User.Identity.GetUserId();
            var user = db.Users.Where(a=>a.Id == UserId).SingleOrDefault();
            var check = db.ApplyForJobs.Where(a => a.jobId == id && a.IsConfirmed==true).ToList();

            if (job.IsConfirmed == false && check.Count()<1)
            {
                job.IsConfirmed = true;
                db.Entry(job).Property("IsConfirmed").IsModified = true;
                db.SaveChanges();
                
                var mail = new MailMessage();
                var loginInfo = new NetworkCredential("devmostafa350@gmail.com", "MOstafa1234_");
                mail.From = new MailAddress(user.Email);
                mail.To.Add(new MailAddress(job.user.Email));
                mail.Subject = "EgyGobs";
                mail.IsBodyHtml = true;
                string body = "اسم المرسل : " + user.UserName + "<br>" +
                    " بريد المرسل : " + user.Email + "<br>" +
                    "عنوان الرسالة :  لقد وافق " + "<b>"+user.UserName +"</b>"+ " على طلبك بالتقدم الى هذا الوظيفة ويرجي سرعة التواصل معه"+ "<br>" +
                    "نص الرسالة : <b>" + "لقد تم قبولك لهذه الوظيفة : " + job.job.JobTitle+" ."+ " يرجى سرعة التواصل معنا على حسابنا الشخصي" + "</b>";
                mail.Body = body;

                var smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = loginInfo;
                smtpClient.Send(mail);
                ViewBag.Mess = "تمت الموافقة بنجاح";

                return View(job);
            }
            else
            {
                ViewBag.Mess = "عفوا لقد سبق ووافقت على هذا الطلب";
            }

            return View();
        }


    }
}