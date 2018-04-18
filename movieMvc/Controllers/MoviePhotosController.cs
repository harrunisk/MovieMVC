using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using movieMvc.Models;



namespace movieMvc.Controllers
{
    [Authorize(Roles = "Admin")]

    public class MoviePhotosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MoviePhotos
        public ActionResult Index()
        {
            var moviePhotosFunc = db.MoviePhotosFunc.Include(m => m.Movie).Include(m => m.Photo);
            return View(moviePhotosFunc.ToList());
        }

        // GET: MoviePhotos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MoviePhotos moviePhotos = db.MoviePhotosFunc.Find(id);
            if (moviePhotos == null)
            {
                return HttpNotFound();
            }
            return View(moviePhotos);
        }

        // GET: MoviePhotos/Create
        public ActionResult Create()
        {
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName");
            ViewBag.PhotoID = new SelectList(db.PhotoFunc, "PhotoID", "PhotoID");
            return View();
        }

        // POST: MoviePhotos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MovieID,PhotoID")] MoviePhotos moviePhotos)
        {
            if (ModelState.IsValid)
            {
                db.MoviePhotosFunc.Add(moviePhotos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName", moviePhotos.MovieID);
            ViewBag.PhotoID = new SelectList(db.PhotoFunc, "PhotoID", "PhotoID", moviePhotos.PhotoID);
            return View(moviePhotos);
        }

        // GET: MoviePhotos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MoviePhotos moviePhotos = db.MoviePhotosFunc.Find(id);
            if (moviePhotos == null)
            {
                return HttpNotFound();
            }
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName", moviePhotos.MovieID);
            ViewBag.PhotoID = new SelectList(db.PhotoFunc, "PhotoID", "PhotoID", moviePhotos.PhotoID);
            return View(moviePhotos);
        }

        // POST: MoviePhotos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MovieID,PhotoID")] MoviePhotos moviePhotos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(moviePhotos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName", moviePhotos.MovieID);
            ViewBag.PhotoID = new SelectList(db.PhotoFunc, "PhotoID", "PhotoID", moviePhotos.PhotoID);
            return View(moviePhotos);
        }

        // GET: MoviePhotos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MoviePhotos moviePhotos = db.MoviePhotosFunc.Find(id);
            if (moviePhotos == null)
            {
                return HttpNotFound();
            }
            return View(moviePhotos);
        }

        // POST: MoviePhotos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MoviePhotos moviePhotos = db.MoviePhotosFunc.Find(id);
            db.MoviePhotosFunc.Remove(moviePhotos);
            db.SaveChanges();
            return RedirectToAction("Index");
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
