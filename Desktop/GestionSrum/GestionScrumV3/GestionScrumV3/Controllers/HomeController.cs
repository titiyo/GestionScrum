using GestionScrumV3.DAL;
using GestionScrumV3.Filters;
using GestionScrumV3.Models;
using Microsoft.Web.WebPages.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using GestionScrumV3.Entity;

namespace GestionScrumV3.Controllers
{
    public class HomeController : Controller
    {
        private GestionScrumContext _context = new GestionScrumContext();
        //
        // GET: /Home/

        public ActionResult Index()
        {
            // Get all projects 
            if (HttpContext.Application.AllKeys.Contains("currentProject"))
            {
                HttpContext.Application.Remove("currentProject");
            }

            HttpContext.Application.Set("projects", _context.Project.Where(x => x.Team.Users.Where(y => y.UserId == WebSecurity.CurrentUserId).Count() > 0).ToList());
            return View();
        }

        public ActionResult Login()
        {
            if (WebSecurity.IsAuthenticated)
                return RedirectToAction("Index");
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (WebSecurity.Login(model.Login, model.Password))
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Wrong Password");
            }
            return View(model);
        }

        public ActionResult Register()
        {
            RegisterViewModel rvm = new RegisterViewModel();
            rvm.Functions = new List<SelectListItem>();
            rvm.Functions.Add(new SelectListItem()
            {
                 Selected = true,
                 Text = "Not Selected",
                 Value = "0"
            });

            foreach(var item in _context.Function.Where(x=>x.Enabled == true))
            {
                rvm.Functions.Add(new SelectListItem(){
                    Text = item.Name,
                    Value = item.FunctionId.ToString()
                });
            }

            return View(rvm);
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid && !WebSecurity.UserExists(model.Login))
            {
                _context.User.Add(new User()
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Login = model.Login,
                    PhoneNumber = model.PhoneNumber,
                    FunctionId = model.FunctionId
                });
                _context.SaveChanges();

                WebSecurity.CreateAccount(model.Login, model.Password);
                Roles.AddUserToRole(model.Login, "User");
                WebSecurity.Login(model.Login, model.Password);
                return RedirectToAction("Index");
            }

            if(WebSecurity.UserExists(model.Login))
            {
                ModelState.AddModelError("ExistingLogin", "The login already exist !");
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Index");
        }

        public JsonResult LoginAvailable(string login)
        {
            var exist = WebSecurity.UserExists(login);
            return Json(!exist, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(PasswordViewModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
