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
    public class ArtistMoviesController : Controller

    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: ArtistMovies
        public ActionResult Index()
        {
            var artistMovies = db.ArtistMovies.Include(a => a.Artist).Include(a => a.Movie);
            return View(artistMovies.ToList());
        }

        // GET: ArtistMovies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtistMovies artistMovies = db.ArtistMovies.Find(id);
            if (artistMovies == null)
            {
                return HttpNotFound();
            }
            return View(artistMovies);
        }

        // GET: ArtistMovies/Create
        public ActionResult Create()
        {
            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName");
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName");
            return View();
        }

        // POST: ArtistMovies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MovieID,ArtistID")] ArtistMovies artistMovies)
        {
            if (ModelState.IsValid)
            {
                db.ArtistMovies.Add(artistMovies);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName", artistMovies.ArtistID);
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName", artistMovies.MovieID);
            return View(artistMovies);
        }

        // GET: ArtistMovies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtistMovies artistMovies = db.ArtistMovies.Find(id);
            if (artistMovies == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName", artistMovies.ArtistID);
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName", artistMovies.MovieID);
            return View(artistMovies);
        }

        // POST: ArtistMovies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MovieID,ArtistID")] ArtistMovies artistMovies)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artistMovies).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName", artistMovies.ArtistID);
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName", artistMovies.MovieID);
            return View(artistMovies);
        }


       
        // GET: ArtistMovies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtistMovies artistMovies = db.ArtistMovies.Find(id);
            if (artistMovies == null)
            {
                return HttpNotFound();
            }
            return View(artistMovies);
        }

        // POST: ArtistMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArtistMovies artistMovies = db.ArtistMovies.Find(id);
            db.ArtistMovies.Remove(artistMovies);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CreateMovieArtist(int movieID)
        {
            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName");
            ViewBag.MovieID = new SelectList(db.MovieFunc.Where(x=>x.MovieID==movieID), "MovieID", "MovieName");
            return View();
        }

        // POST: ArtistMovies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMovieArtist([Bind(Include = "Id,MovieID,ArtistID")] ArtistMovies artistMovies,
            ArtistMovies artistMovies2, ArtistMovies artistMovies3, ArtistMovies artistMovies4, ArtistMovies artistMovies5,
            string ArtistID2, string ArtistID3, string ArtistID4, string ArtistID5)
        {
            artistMovies2.ArtistID = Convert.ToInt32(ArtistID2);
            artistMovies3.ArtistID = Convert.ToInt32(ArtistID3);
            artistMovies4.ArtistID = Convert.ToInt32(ArtistID4);
            artistMovies5.ArtistID = Convert.ToInt32(ArtistID5);


            if (ModelState.IsValid)
            {
                db.ArtistMovies.Add(artistMovies);
                db.ArtistMovies.Add(artistMovies2);
                db.ArtistMovies.Add(artistMovies3);
                db.ArtistMovies.Add(artistMovies4);
                db.ArtistMovies.Add(artistMovies5);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName", artistMovies.ArtistID);
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName", artistMovies.MovieID);
            return View(artistMovies);
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
