using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using movieMvc.Models;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using Microsoft.AspNet.Identity.EntityFramework;

namespace movieMvc.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";


            if(User.Identity.IsAuthenticated)
            { var user = User.Identity;
                ViewBag.Name = user.Name;


                ViewBag.displayMenu = "No";
                if(isAdminUser())
                {
                    ViewBag.displayMenu = "Yes";



                }

            }

            var userId = User.Identity.GetUserId();

            

            var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();

            //login olmuş kullanıcının UserMovie içindeki movielerin id lerini çektil
            var UserMovieList = bdUsers.UserMovie.Where(x => x.UserId == userId).Select(x => x.MovieID);
            //login olmuş kullanıcının UserArtist içindeki artistlerinin idlerini çektik
            var UserArtistList = bdUsers.UserArtist.Where(x => x.UserId == userId).Select(x => x.ArtistID);

            //çektiğimiz movie idler ile tüm movies içindeki eşleşenleri çağırdık 
            var UserMoviesContent = bdUsers.MovieFunc.Where(a => UserMovieList.Any(x => x == a.MovieID));
            //çiektipimiz artist idler ile tüm artistler içinde eşleşenleri çağırdık.
            var UserArtistContent = bdUsers.Artist.Where(a => UserArtistList.Any(x => x == a.ArtistID));
           
           


            var model = new IndexViewModel
            {
                UserFavoriteArtistList= UserArtistContent,
                UserFavoriteMovieList = UserMoviesContent,
                Id = userId,
                Birth=userImage.Birth,
                RegisterDate=userImage.RegisterDate,
                UserMovie=userImage.UserMovie,
                UserArtist=userImage.UserArtist,
                 Sex=userImage.Sex,                
                HasPassword = HasPassword(),
                Mail = await UserManager.GetEmailAsync(userId),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
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

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserName,Id,Sex,Birth,Email,UserPhoto,SecurityStamp,PasswordHash,RegisterDate")] ApplicationUser user)
        {

            try
            {
                if (ModelState.IsValid)
                {
                   
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
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
            return View(user);
        }
        
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }







        public ActionResult DeleteUserMovie(int? MovieID)
        {

            var userId = User.Identity.GetUserId();

            var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            

           

            //login olan kullanıcının favori filmleri
            var UsersFavoriteMovies = bdUsers.UserMovie.Where(x => x.UserId == userId).Select(x => x.MovieID);
            //favori filmlerden bize gelen idye göre silinecek olan 
            var willDelete = bdUsers.UserMovie.Where(x => x.MovieID == MovieID).FirstOrDefault();


            if (willDelete == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMovie userMovie = db.UserMovie.Find(MovieID);
            if (willDelete == null)
            {
                return HttpNotFound();
            }
            return View(userMovie);
        }

        // POST: UserMovies/Delete/5
        [HttpPost, ActionName("DeleteUserMovie")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserMovieConfirmed(int MovieID)
        {
            var userId = User.Identity.GetUserId();
            var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            //login olan kullanıcının favori filmleri
            var UsersFavoriteMovies = bdUsers.UserMovie.Where(x => x.UserId == userId);
            //favori filmlerden bize gelen idye göre silinecek olan
            //burada döngü kullanma sebebi favoritemovie'de eklerken aynı filmi iki kere ekleyebiliyor bazen gösterirken bir kere gösteriyor
            //ama bir kere silsende 2 kere eklemişse yine gösteriyor
            // o yüzden silerken hepsini siliyoruz
            //çözüm en başta 2 kere eklemesini engellemek olacak. 
            var willDelete = UsersFavoriteMovies.Where(x => x.MovieID == MovieID);

            foreach(var item in willDelete)
            {
                UserMovie userMovie = db.UserMovie.Find(item.Id);
                db.UserMovie.Remove(userMovie);
                db.SaveChanges();

            }
           
            return RedirectToAction("Index");
        }


        
        public ActionResult AddUserMovie(int movieID)
        {
            var userId = User.Identity.GetUserId();
            var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            var MovieWillAdd = bdUsers.MovieFunc.Where(x => x.MovieID == movieID);
            UserMovie userMovie = new UserMovie
            {
                MovieID = movieID,
                UserId = userId
            };

            db.UserMovie.Add(userMovie);
            db.SaveChanges();



            return RedirectToAction("Index");
        }








        public ActionResult DeleteUserArtist(int? ArtistID)
        {

            var userId = User.Identity.GetUserId();

            var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();




            //login olan kullanıcının favori artistleri
            var UsersFavoriteArtists = bdUsers.UserArtist.Where(x => x.UserId == userId).Select(x => x.ArtistID);
            //favori artistlerden bize gelen idye göre silinecek olan 
            var willDelete = bdUsers.UserArtist.Where(x => x.ArtistID == ArtistID).FirstOrDefault();


            if (willDelete == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserArtist userArtist = db.UserArtist.Find(willDelete.Id);
            if (willDelete == null)
            {
                return HttpNotFound();
            }
            return View(userArtist);
        }

        // POST: UserMovies/Delete/5
        [HttpPost, ActionName("DeleteUserArtist")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserArtistConfirmed(int ArtistID)
        {
            var userId = User.Identity.GetUserId();
            var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            //login olan kullanıcının favori artistleri
            var UsersFavoriteArtists = bdUsers.UserArtist.Where(x => x.UserId == userId);
            //favori filmlerden bize gelen idye göre silinecek olan 
            var willDelete = UsersFavoriteArtists.Where(x => x.ArtistID == ArtistID);

                foreach (var item in willDelete)
            {
                UserArtist userArtist = db.UserArtist.Find(item.Id);
                db.UserArtist.Remove(userArtist);
                db.SaveChanges();
                
            }

          
            return RedirectToAction("Index");
        }

        public ActionResult AddUserArtist(int artistID)
        {
            var userId = User.Identity.GetUserId();
            var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();

            UserArtist userArtist = new UserArtist
            {
                ArtistID = artistID,
                UserId = userId
            };

            db.UserArtist.Add(userArtist);
            db.SaveChanges();



            return RedirectToAction("Index");
        }

        public ActionResult GetCommentMovie(int MovieID)
        {
            //şu an sadece kullanıcın bir yorumu gönderiliyor kullanıcın diğer yorumlarıda gönderilmeli.

            var userId = User.Identity.GetUserId();

            var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            //kullanıcının favori filmlerini çektik 
            var userMovies = bdUsers.UserMovie.Where(x => x.UserId == userId);
            //favori filmler arasından yorumunu çekmek istediğimiz filmleri çektik
            var userFavoriteMovies = userMovies.Where(x => x.MovieID == MovieID).FirstOrDefault();


            var result = userFavoriteMovies.Comment;
            

            return Content(result);
          

        }
        public ActionResult GetCommentArtist(int ArtistID)
        {
            //şu an sadece kullanıcın bir yorumu gönderiliyor kullanıcın diğer yorumlarıda gönderilmeli.

            var userId = User.Identity.GetUserId();

            var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            //kullanıcının favori artistlerini  çektik 
            var userArtists = bdUsers.UserArtist.Where(x => x.UserId == userId);
            //favori artistler arasından yorumunu çekmek istediğimiz artisti çektik
            var userFavoriteMovies = userArtists.Where(x => x.ArtistID == ArtistID).FirstOrDefault();


            var result = userFavoriteMovies.Comment;


            return Content(result);


        }


        #endregion
    }
}