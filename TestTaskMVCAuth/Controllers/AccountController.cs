using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;
using TestTaskMVCAuth.Models;

namespace TestTaskMVCAuth.Controllers
{
    public class AccountController : Controller
    {
        private AuthUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AuthUserManager>(); }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        public ActionResult Login(string urlStr)
        {
            ViewBag.urlStr = urlStr;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string urlStr)
        {
            if (ModelState.IsValid)
            {
                AuthUser user =
                    await UserManager.FindAsync(model.Email, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Wrong login or password");
                }
                else
                {
                    ClaimsIdentity claims =
                        await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                    AuthenticationManager.SignOut();

                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claims);

                    if (String.IsNullOrEmpty(urlStr))
                    {
                        return RedirectToAction("Index", "Account");
                    }                    

                    return Redirect(urlStr);
                }
            }

            ViewBag.urlStr = urlStr;
            return View(model);
        }
        
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        [CustomAuthorize(Roles = "admin, user1, user2")]
        public ActionResult Index()
        {
            return View("~/Views/Account/Index.cshtml");
        }

        [CustomAuthorize(Roles = "admin, user1")]
        public ActionResult Admin()
        {
            return View("~/Views/Account/admin.cshtml");
        }

        [CustomAuthorize(Roles = "admin, user1")]
        public ActionResult User1()
        {
            return View("~/Views/Account/view1.cshtml");
        }

        [CustomAuthorize(Roles = "admin, user2")]
        public ActionResult User2()
        {
            return View("~/Views/Account/view2.cshtml");
        }
        
        public ActionResult AccessDenied()
        {
            return View("~/Views/Account/AccessDenied.cshtml");
        }

        public ActionResult RequestLogin()
        {
            return View("~/Views/Account/RequestLogin.cshtml");
        }
    }

    /// <summary>
    /// For custom authorization attribute
    /// </summary>
    public class CustomAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                httpContext.Response.Redirect("/Account/RequestLogin");
                return false;
            }

            if (!isAuthorized)
            {
                httpContext.Response.Redirect("/Account/AccessDenied");
                return false;
            }

            return true;
        }
    }
}