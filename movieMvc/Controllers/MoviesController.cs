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
using System.Data.Entity.Validation;
using System.Diagnostics;
using Microsoft.AspNet.Identity.Owin;
using System.IO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace movieMvc.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MoviesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Movies
        public ActionResult Index()
        {

         

            var movie = db.MovieFunc.Include(t => t.MoviePhoto);

            return View(movie.ToList());
        }


        [AllowAnonymous]
        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {

            
            var userId = User.Identity.GetUserId();
            if (userId != null)
            {

                var UserMovie = db.UserMovie.Where(x => x.MovieID == id && x.UserId==userId).FirstOrDefault();
                
                
                if (UserMovie!=null)
                {
                    ViewData["Result"] = true;

                }
                else if (UserMovie==null)
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
            else {
                ViewData["Result"] = false;
                ViewData["Role"] = "Anonymous";
            }


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.MovieFunc.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }

            var temp = db.MoviePhotosFunc.Where(x => x.MovieID == id).Select(x=>x.PhotoID);
            var MoviePhotosList = db.PhotoFunc.Where(a => temp.Any(x => x == a.PhotoID));
            ViewData["MoviePhotosList"] = MoviePhotosList.ToList();


            var MovieArtistListMovieIDs = db.ArtistMovies.Where(x => x.MovieID == id).Select(x => x.ArtistID);
            var MovieArtistList = db.Artist.Where(a => MovieArtistListMovieIDs.Any(x => x == a.ArtistID));
            ViewData["MovieArtistList"] = MovieArtistList.ToList();




            return View(movie);
        }
        [Authorize(Roles = "Admin")]
        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MovieID,MovieName,MovieReleaseDate,MovieGeneralScore,MovieGenre,MovieDirector,MovieTrailer,MovieInformation,MovieDuration")] Movie movie, Photo photo, MoviePhotos moviePhoto)
        {
            WebImage image = WebImage.GetImageFromRequest();
            byte[] toPutInDb = image.GetBytes();
            photo.PhotoContent = toPutInDb;
            moviePhoto.PhotoID = photo.PhotoID;
            moviePhoto.MovieID = movie.MovieID;


            try
            {
                if (ModelState.IsValid)
                {
                    db.MoviePhotosFunc.Add(moviePhoto);
                    db.PhotoFunc.Add(photo);
                    db.MovieFunc.Add(movie);
                    db.SaveChanges();
                    return RedirectToAction("CreateMovieArtist", "ArtistMovies", new { MovieID = movie.MovieID });
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }

            return View(movie);
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.MovieFunc.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieID,MovieName,MovieReleaseDate,MovieGeneralScore,MovieGenre,MovieDirector,MovieTrailer,MovieInformation,MovieDuration")] Movie movie, HttpPostedFileBase[] uploadImages)
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

                    MoviePhotos mv = new MoviePhotos() { PhotoID = last2.PhotoID, MovieID =movie.MovieID };

                    db.MoviePhotosFunc.Add(mv);
                    db.SaveChanges();


                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.MovieFunc.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.MovieFunc.Find(id);
            db.MovieFunc.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public FileContentResult MoviesPhotos(int MovieID)
        {

            if (MovieID == null)
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

                var userImage = bdUsers.MoviePhotosFunc.Where(x => x.MovieID == MovieID).FirstOrDefault();

                var lastUserImage = bdUsers.PhotoFunc.Where(x => x.PhotoID == userImage.PhotoID).FirstOrDefault();

                return new FileContentResult(lastUserImage.PhotoContent, "image/jpeg");
            }
           
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
        public FileContentResult BringPhoto(int PhotoID)
        {

            if (PhotoID == null)
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


                var lastUserImage = bdUsers.PhotoFunc.Where(x => x.PhotoID == PhotoID).FirstOrDefault();

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
