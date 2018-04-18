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
    
    public class UserArtistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserArtists
        public ActionResult Index()
        {
            var userArtists = db.UserArtist.Include(u => u.ApplicationUser).Include(u => u.Artist);
            return View(userArtists.ToList());
        }

        [AllowAnonymous]
        // GET: UserArtists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserArtist userArtist = db.UserArtist.Find(id);
            if (userArtist == null)
            {
                return HttpNotFound();
            }
            return View(userArtist);
        }

        // GET: UserArtists/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName");
            return View();
        }

        // POST: UserArtists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,ArtistID,Score,Comment,Favorite")] UserArtist userArtist)
        {
            if (ModelState.IsValid)
            {
                db.UserArtist.Add(userArtist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userArtist.UserId);
            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName", userArtist.ArtistID);
            return View(userArtist);
        }

        // GET: UserArtists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserArtist userArtist = db.UserArtist.Find(id);
            if (userArtist == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userArtist.UserId);
            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName", userArtist.ArtistID);
            return View(userArtist);
        }

        // POST: UserArtists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,ArtistID,Score,Comment,Favorite")] UserArtist userArtist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userArtist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userArtist.UserId);
            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName", userArtist.ArtistID);
            return View(userArtist);
        }

        // GET: UserArtists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserArtist userArtist = db.UserArtist.Find(id);
            if (userArtist == null)
            {
                return HttpNotFound();
            }
            return View(userArtist);
        }

        // POST: UserArtists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserArtist userArtist = db.UserArtist.Find(id);
            db.UserArtist.Remove(userArtist);
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
