using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using movieMvc.Models;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace movieMvc.Controllers
{    
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        

        public ActionResult Index()

        {           


            var movies = db.MovieFunc.ToList();
            //yüksekten alçağa
            var moviesOrderedByDescedingScore = db.MovieFunc.OrderByDescending(x => x.MovieGeneralScore);
            ViewData["moviesOrderedByDescedingScore"] = moviesOrderedByDescedingScore.ToList();
            
            var moviesOrderedByDate = db.MovieFunc.OrderByDescending(x => x.MovieReleaseDate);
            ViewData["moviesOrderedByDate"] = moviesOrderedByDate.ToList();

            return View(movies);
        }

        public ActionResult Change(String LanguageAbbrevation)
        {
            if (LanguageAbbrevation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbrevation);
            }

            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = LanguageAbbrevation;
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index");



        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Series()
        {
            return View();
        }
        public ActionResult News()
        {
            return View();
        }
        public ActionResult Genres()
        {
            return View();
        }
        public ActionResult ShortCodes()
        {
            return View();
        }
        public ActionResult List()
        {
            return View();
        }
        public ActionResult Faq()
        {
            return View();
        }
        public ActionResult Horror()
        {
            return View();
        }
        public ActionResult Comedy()
        {
            return View();
        }
        public ActionResult Icons()
        {
            return View();
        }
        public ActionResult Contact2()
        {
            return View();
        }

        public FileContentResult UserPhotos()
        {
            if (User.Identity.IsAuthenticated)
            {
                String userId = User.Identity.GetUserId();

                if (userId == null)
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
                // to get the user details to load user Image 
                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();

                return new FileContentResult(userImage.UserPhoto, "image/jpeg");
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/images/1.jpg");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");

            }
        }

    }
}