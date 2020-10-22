using EgyGobs.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;
using WebApplication1.Models;
namespace EgyGobs.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext DB = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListUsers()
        {
            var Users = DB.Users.ToList();
            return View(Users);

        }

        public ActionResult BlockUser(string id)
        {
            var user = (ApplicationUser)UserManager.FindById(id);

            user.EmailConfirmed = false;
            UserManager.Update(user);

            return RedirectToAction("ListUsers");

        }
        public ActionResult UnBlockUser(string id)
        {

            var user = (ApplicationUser)UserManager.FindById(id);

            user.EmailConfirmed = true;
            UserManager.Update(user);

            return RedirectToAction("ListUsers");
        }

        public ActionResult ListJobs()
        {
            var Job = DB.Jobs.ToList();
            return View(Job);

        }

        public ActionResult DeleteJob(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = DB.Jobs.Find(id);
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
            Job job = DB.Jobs.Find(id);
            ApplyForJob applied = DB.ApplyForJobs.Where(a => a.jobId == id).SingleOrDefault();
            if(applied ==null)
            {
                DB.Jobs.Remove(job);
                DB.SaveChanges();

                return RedirectToAction("ListJobs");

            }
            else
            {
                ViewBag.Mess = "لا يمكن حذف هذه الوظيفة نظرا لوجود تعاقد بها من شخص ما";
                return View(job);
            }
           
            
        }


    }
}