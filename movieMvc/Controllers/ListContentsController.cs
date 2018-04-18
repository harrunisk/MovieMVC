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
    public class ListContentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ListContents
        public ActionResult Index()
        {
            var listContent = db.ListContent.Include(l => l.List).Include(l => l.Movie);
            return View(listContent.ToList());
        }

        // GET: ListContents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListContent listContent = db.ListContent.Find(id);
            if (listContent == null)
            {
                return HttpNotFound();
            }
            return View(listContent);
        }

        // GET: ListContents/Create
        public ActionResult Create()
        {
            ViewBag.ListID = new SelectList(db.List, "ListID", "ListTitle");
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName");
            return View();
        }

        // POST: ListContents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MovieID,ListID")] ListContent listContent)
        {
            if (ModelState.IsValid)
            {
                db.ListContent.Add(listContent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ListID = new SelectList(db.List, "ListID", "ListTitle", listContent.ListID);
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName", listContent.MovieID);
            return View(listContent);
        }

     

        public ActionResult CreatePrivate(int listID)
        {


            ViewBag.ListID = new SelectList(db.List.Where(x=>x.ListID==listID), "ListID", "ListTitle");
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName");
            return View();
        }

        // POST: ListContents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePrivate([Bind(Include = "Id,MovieID,ListID")] ListContent listContent,   ListContent listContent2, ListContent listContent3, ListContent listContent4, ListContent listContent5,int listID,string MovieID2, string MovieID3, string MovieID4, string MovieID5)
        {

            listContent2.MovieID = Convert.ToInt32 (MovieID2);
            listContent3.MovieID = Convert.ToInt32(MovieID3);
            listContent4.MovieID = Convert.ToInt32(MovieID4);
            listContent5.MovieID = Convert.ToInt32(MovieID5);





            if (ModelState.IsValid)
            {
                db.ListContent.Add(listContent);
                db.ListContent.Add(listContent2);
                db.ListContent.Add(listContent3);
                db.ListContent.Add(listContent4);
                db.ListContent.Add(listContent5);



                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ListID = new SelectList(db.List, "ListID", "ListTitle", listContent.ListID);
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName", listContent.MovieID);
            return View(listContent);
        }

        // GET: ListContents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListContent listContent = db.ListContent.Find(id);
            if (listContent == null)
            {
                return HttpNotFound();
            }
            ViewBag.ListID = new SelectList(db.List, "ListID", "ListTitle", listContent.ListID);
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName", listContent.MovieID);
            return View(listContent);
        }

        // POST: ListContents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MovieID,ListID")] ListContent listContent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listContent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ListID = new SelectList(db.List, "ListID", "ListTitle", listContent.ListID);
            ViewBag.MovieID = new SelectList(db.MovieFunc, "MovieID", "MovieName", listContent.MovieID);
            return View(listContent);
        }

        // GET: ListContents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListContent listContent = db.ListContent.Find(id);
            if (listContent == null)
            {
                return HttpNotFound();
            }
            return View(listContent);
        }

        // POST: ListContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListContent listContent = db.ListContent.Find(id);
            db.ListContent.Remove(listContent);
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
