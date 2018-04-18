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
    public class UserMoviesController : Controller
    {
       
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserMovies
        public ActionResult Index()
        {
            var userMovies = db.UserMovie.Include(u => u.ApplicationUser).Include(u => u.Movie);
            return View(userMovies.ToList());
        }

        // GET: UserMovies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMovie userMovie = db.UserMovie.Find(id);
            if (userMovie == null)
            {
                return HttpNotFound();
            }
            return View(userMovie);
        }

        // GET: UserMovies/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName");
            return View();
        }

        // POST: UserMovies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,MovieID,Score,Comment,Favorite")] UserMovie userMovie)
        {
            if (ModelState.IsValid)
            {
                db.UserMovie.Add(userMovie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userMovie.UserId);
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName", userMovie.MovieID);
            return View(userMovie);
        }

        // GET: UserMovies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMovie userMovie = db.UserMovie.Find(id);
            if (userMovie == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userMovie.UserId);
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName", userMovie.MovieID);
            return View(userMovie);
        }

        // POST: UserMovies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,MovieID,Score,Comment,Favorite")] UserMovie userMovie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userMovie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userMovie.UserId);
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName", userMovie.MovieID);
            return View(userMovie);
        }

        // GET: UserMovies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMovie userMovie = db.UserMovie.Find(id);
            if (userMovie == null)
            {
                return HttpNotFound();
            }
            return View(userMovie);
        }

        // POST: UserMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserMovie userMovie = db.UserMovie.Find(id);
            db.UserMovie.Remove(userMovie);
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
