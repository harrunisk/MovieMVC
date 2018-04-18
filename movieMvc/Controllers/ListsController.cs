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
{   [Authorize(Roles ="Admin")]
    public class ListsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Lists
        public ActionResult Index()
        {
            return View(db.List.ToList());
        }
        [AllowAnonymous]
        public ActionResult Lists() {
            return View(db.List.ToList());

        }
        [AllowAnonymous]
        // GET: Lists/Details/5
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
            List list = db.List.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }

            var temp = db.ListPhoto.Where(x => x.ListID == id).Select(x => x.PhotoID).FirstOrDefault();
            var ListPhotoOne = db.PhotoFunc.Where(x => x.PhotoID == temp).FirstOrDefault();


            ViewData["ListPhoto"] = ListPhotoOne.PhotoID;
            return View(list);
        }

        // GET: Lists/Create
        public ActionResult Create()
        {
            var Movies = db.MovieFunc.ToList();
            ViewData["Movies"] = Movies;

            
            return View();
        }

        // POST: Lists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ListID,ListTitle")] List list, Photo photo, ListPhoto listPhoto)
        {
            var Movies = db.MovieFunc.ToList();
            ViewData["Movies"] = Movies;
            WebImage image = WebImage.GetImageFromRequest();
            byte[] toPutInDb = image.GetBytes();
            photo.PhotoContent = toPutInDb;
            listPhoto.PhotoID = photo.PhotoID;
            listPhoto.ListID = list.ListID;
            if (ModelState.IsValid)
            {
                db.ListPhoto.Add(listPhoto);
                db.PhotoFunc.Add(photo);
                db.List.Add(list);
                db.SaveChanges();
                return RedirectToAction("CreatePrivate","ListContents",new {listID=list.ListID});
            }

            return View(list);
        }

        // GET: Lists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List list = db.List.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: Lists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ListID,ListTitle")] List list)
        {
            if (ModelState.IsValid)
            {
                db.Entry(list).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(list);
        }

        // GET: Lists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List list = db.List.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }
            return View(list);
        }

        // POST: Lists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List list = db.List.Find(id);
            db.List.Remove(list);
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
