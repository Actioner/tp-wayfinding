using System.Web.Mvc;
using System.Web.Security;
using Simple.Data;
using TP.Wayfinding.Site.Components.Services;
using TP.Wayfinding.Site.Models;
using TP.Wayfinding.Site.Models.Account;

namespace TP.Wayfinding.Site.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IEncryptionService _encryptionService;

        public AccountController()
        {
            _encryptionService = new EncryptionService();
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                SignIn(model.UserName, model.RememberMe);
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";

            ViewBag.ReturnUrl = Url.Action("Manage");
            return View(new ManageModel
            {
                UserName = User.Identity.Name
            });
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ManageModel model)
        {
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (ModelState.IsValid)
            {
                var db = Database.Open();
                var user = db.User.FindByEmail(User.Identity.Name);
                user.Password = _encryptionService.EncryptPassword(model.NewPassword);
                db.User.Update(user);

                return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private void SignIn(string userName, bool isPersistent)
        {
            FormsAuthentication.SetAuthCookie(userName, isPersistent);
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}