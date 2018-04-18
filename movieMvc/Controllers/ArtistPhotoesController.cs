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
    public class ArtistPhotoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ArtistPhotoes
        public ActionResult Index()
        {
            var artistPhoto = db.ArtistPhoto.Include(a => a.Artist).Include(a => a.Photo);
            return View(artistPhoto.ToList());
        }

        // GET: ArtistPhotoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtistPhoto artistPhoto = db.ArtistPhoto.Find(id);
            if (artistPhoto == null)
            {
                return HttpNotFound();
            }
            return View(artistPhoto);
        }

        // GET: ArtistPhotoes/Create
        public ActionResult Create()
        {
            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName");
            ViewBag.PhotoID = new SelectList(db.PhotoFunc, "PhotoID", "PhotoID");
            return View();
        }

        // POST: ArtistPhotoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ArtistID,PhotoID")] ArtistPhoto artistPhoto)
        {
            if (ModelState.IsValid)
            {
                db.ArtistPhoto.Add(artistPhoto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName", artistPhoto.ArtistID);
            ViewBag.PhotoID = new SelectList(db.PhotoFunc, "PhotoID", "PhotoID", artistPhoto.PhotoID);
            return View(artistPhoto);
        }

        // GET: ArtistPhotoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtistPhoto artistPhoto = db.ArtistPhoto.Find(id);
            if (artistPhoto == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName", artistPhoto.ArtistID);
            ViewBag.PhotoID = new SelectList(db.PhotoFunc, "PhotoID", "PhotoID", artistPhoto.PhotoID);
            return View(artistPhoto);
        }

        // POST: ArtistPhotoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ArtistID,PhotoID")] ArtistPhoto artistPhoto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(artistPhoto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArtistID = new SelectList(db.Artist, "ArtistID", "ArtistName", artistPhoto.ArtistID);
            ViewBag.PhotoID = new SelectList(db.PhotoFunc, "PhotoID", "PhotoID", artistPhoto.PhotoID);
            return View(artistPhoto);
        }

        // GET: ArtistPhotoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArtistPhoto artistPhoto = db.ArtistPhoto.Find(id);
            if (artistPhoto == null)
            {
                return HttpNotFound();
            }
            return View(artistPhoto);
        }

        // POST: ArtistPhotoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArtistPhoto artistPhoto = db.ArtistPhoto.Find(id);
            db.ArtistPhoto.Remove(artistPhoto);
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
