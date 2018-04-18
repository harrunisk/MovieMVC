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
using System.IO;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace movieMvc.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ArtistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Artists
        public ActionResult Index()
        {
            return View(db.Artist.ToList());
        }
        [AllowAnonymous]

        // GET: Artists/Details/5
        public ActionResult Details(int? id)
        {
            var userId = User.Identity.GetUserId();
            var UsersFavoriteArtists = db.UserArtist.Where(x => x.UserId == userId).Select(x => x.ArtistID);
            var UserArtist = db.UserArtist.Where(x => x.ArtistID == id).FirstOrDefault();
           
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
            Artist artist = db.Artist.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            var temp = db.ArtistPhoto.Where(x => x.ArtistID == id).Select(x => x.PhotoID);
            var ArtistPhotoList = db.PhotoFunc.Where(a => temp.Any(x => x == a.PhotoID));


            ViewData["ArtistPhotoList"] = ArtistPhotoList.ToList();
            return View(artist);
        }

        // GET: Artists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArtistID,ArtistName,ArtistBirth,ArtistPlaceOfBirth,ArtistBiography,ArtisGeneralScore")] Artist artist, Photo photo, ArtistPhoto artistPhoto)
        {

            WebImage ArtistImage = WebImage.GetImageFromRequest();
            byte[] toPutInDb = ArtistImage.GetBytes();
            photo.PhotoContent = toPutInDb;
            artistPhoto.PhotoID = photo.PhotoID;
            artistPhoto.ArtistID = artist.ArtistID;

            if (ModelState.IsValid)
            {
                db.PhotoFunc.Add(photo);
                db.ArtistPhoto.Add(artistPhoto);
                db.Artist.Add(artist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(artist);
        }

        // GET: Artists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artist.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArtistID,ArtistName,ArtistBirth,ArtistPlaceOfBirth,ArtistBiography,ArtisGeneralScore")] Artist artist, HttpPostedFileBase[] uploadImages)
        {
            foreach (var image in uploadImages)
            {
                if (image.ContentLength > 0)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(image.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(image.ContentLength);
                    }
                    var headerImage = new Photo
                    {
                        PhotoContent = imageData


                    };
                    db.PhotoFunc.Add(headerImage);
                    db.SaveChanges();

                    var last2 = db.PhotoFunc.OrderByDescending(x => x.PhotoID).First();

                    ArtistPhoto av = new ArtistPhoto() { PhotoID = last2.PhotoID, ArtistID = artist.ArtistID };

                    db.ArtistPhoto.Add(av);
                    db.SaveChanges();


                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(artist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(artist);
        }

        // GET: Artists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Artist artist = db.Artist.Find(id);
            if (artist == null)
            {
                return HttpNotFound();
            }
            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Artist artist = db.Artist.Find(id);
            db.Artist.Remove(artist);
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

        [AllowAnonymous]
        public FileContentResult ArtistPhotos(int ArtistID)
        {

            if (ArtistID == null)
            {
                string fileName = HttpContext.Server.MapPath(@"~images/1.jpg");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);

                return File(imageData, "image/png");

            }


            else
            {

                // to get the user details to load user Image 
                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

                var userImage = bdUsers.ArtistPhoto.Where(x => x.ArtistID == ArtistID).FirstOrDefault();

                var lastUserImage = bdUsers.PhotoFunc.Where(x => x.PhotoID == userImage.PhotoID).FirstOrDefault();

                return new FileContentResult(lastUserImage.PhotoContent, "image/jpeg");
            }

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
