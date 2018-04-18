using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using movieMvc.Models;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace movieMvc.Controllers
{
   
     [Authorize(Roles = "Admin")]

    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: News
        public ActionResult Index()
        {
            return View(db.News.ToList());
        }
        [AllowAnonymous]
        public ActionResult Index2()
        {


            return View(db.News.ToList());


        }
        [AllowAnonymous]
        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            var userId = User.Identity.GetUserId();
           

            if (userId != null)
            {

                var UserArtistResult = db.UserArtist.Where(x => x.ArtistID == id && x.UserId == userId).FirstOrDefault();


                if (UserArtistResult != null)
                {
                    ViewData["Result"] = true;

                }
                else if (UserArtistResult == null)
                {

                    ViewData["Result"] = false;


                }

                ViewData["Role"] = "Anonymous";

                if (User.Identity.IsAuthenticated)
                {
                    ViewData["Role"] = "User";
                    if (isAdminUser())
                    {
                        ViewData["Role"] = "Admin";



                    }

                }
            }
            else
            {
                ViewData["Result"] = false;
                ViewData["Role"] = "Anonymous";
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            var temp = db.NewsPhoto.Where(x => x.NewsID == id).Select(x => x.PhotoID).FirstOrDefault();
            var NewsPhotoOne = db.PhotoFunc.Where(x => x.PhotoID == temp).FirstOrDefault();


            ViewData["NewsPhoto"] = NewsPhotoOne.PhotoID;
            return View(news);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NewsID,NewsContent,NewsTitle,NewsDate")] News news, Photo photo, NewsPhoto newsPhoto)
        {
            WebImage image = WebImage.GetImageFromRequest();
            byte[] toPutInDb = image.GetBytes();
            photo.PhotoContent = toPutInDb;
            newsPhoto.PhotoID = photo.PhotoID;
            newsPhoto.NewsID = news.NewsID;

            if (ModelState.IsValid)
            {
                db.NewsPhoto.Add(newsPhoto);
                db.PhotoFunc.Add(photo);
                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(news);
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NewsID,NewsContent,NewsTitle,NewsDate")] News news)
        {
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private bool isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());


                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
