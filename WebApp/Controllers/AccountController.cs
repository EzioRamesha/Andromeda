using BusinessObject.Identity;
using BusinessObject;
using DataAccess.Entities.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Services.Identity;
using Shared;
using Shared.DataAccess;
using Shared.Trails;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
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
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string name = model.Email;
            UserBo userBo = UserService.FindByUsernameOrEmail(name);
            if (userBo != null)
            {
                name = userBo.UserName;
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(name, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    UserService.UpdateLastLoginAt(ref userBo, GetIpAddress());
                    if (userBo.PasswordExpiresAt < DateTime.Now)
                        return RedirectToAction("ChangePassword", "Manage", new { passwordExpired = true });
                    else if (!string.IsNullOrEmpty(returnUrl))
                        return RedirectToLocal(returnUrl);
                    return RedirectToAction("Index", "Home");
                case SignInStatus.LockedOut:
                    if (userBo != null && userBo.Status != UserBo.StatusSuspend)
                    {
                        TrailObject trail = GetNewTrailObject();
                        userBo.Status = UserBo.StatusSuspend;
                        Result = UserService.UpdateStatus(ref userBo, ref trail);

                        CreateTrail(
                            userBo.Id,
                            "Suspend User"
                        );
                    }
                    ModelState.AddModelError("", MessageBag.UserSuspended);
                    return View(model);
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    if (userBo != null && userBo.Status == UserBo.StatusSuspend)
                    {
                        ModelState.AddModelError("", MessageBag.UserSuspended);
                    }
                    else
                    {
                        string message = SignInManager.getError() ?? MessageBag.InvalidLogin;
                        ModelState.AddModelError("", message);
                    }
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId == 0 || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string name = model.Email;
                UserBo userBo = UserService.FindByUsernameOrEmail(name);

                if (userBo == null)
                {
                    ModelState.AddModelError("", MessageBag.NoRecordFound);
                    return View(model);
                }

                if (userBo.LoginMethod == UserBo.LoginMethodAD)
                {
                    ModelState.AddModelError("", MessageBag.RequestPasswordDenied);
                    return View(model);
                }

                TrailObject trail = GetNewTrailObject();
                int passwordLength = int.Parse(Util.GetConfig("RandomPasswordLength"));
                string password = Util.GenerateRandomString(passwordLength, passwordLength);

                Result = UserService.UpdatePassword(ref userBo, password, ref trail);
                if (Result.Valid)
                {
                    GetNewEmail(EmailBo.TypeResetTemporaryPassword, userBo.Email, userBo.Id);
                    EmailBo.AddData(password);
                    EmailBo.CreatedById = UserBo.DefaultSuperUserId;
                    EmailBo.UpdatedById = UserBo.DefaultSuperUserId;
                    bool success = GenerateMail();
                    SaveEmail(ref trail);

                    CreateTrail(
                        userBo.Id,
                        "Forget Password"
                    );

                    if (!success)
                        return View(model);

                    return RedirectToAction("Login");
                }
                AddResult(Result);

                return View(model);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == 0)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            UserTrailService.CreateLogoutTrail(AuthUser, GetIpAddress());
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        //
        // GET: /Account/RequestPassword
        [AllowAnonymous]
        public ActionResult RequestPassword(string returnUrl)
        {
            ViewBag.Disabled = false;
            bool activeDirectoryEnabled = Boolean.Parse(Util.GetConfig("ActiveDirectoryEnabled"));
            if (activeDirectoryEnabled)
            {
                ModelState.AddModelError("", "Request Password is not available when Active Directory is enabled");
                ViewBag.Disabled = true;
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/RequestPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult RequestPassword(RequestPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string name = model.Email;
            UserBo userBo = UserService.FindByUsernameOrEmail(name);

            if (userBo == null)
            {
                ModelState.AddModelError("", MessageBag.NoRecordFound);
                return View(model);
            }

            if (userBo.LoginMethod == UserBo.LoginMethodPassword)
            {
                ModelState.AddModelError("", MessageBag.RequestPasswordDenied);
                return View(model);
            }

            TrailObject trail = GetNewTrailObject();
            int passwordLength = int.Parse(Util.GetConfig("RandomPasswordLength"));
            string password = Util.GenerateRandomString(passwordLength, passwordLength);

            Result = UserService.UpdatePassword(ref userBo, password, ref trail);
            if (Result.Valid)
            {
                GetNewEmail(EmailBo.TypeRequestTemporaryPassword, userBo.Email, userBo.Id);
                EmailBo.CreatedById = UserBo.DefaultSuperUserId;
                EmailBo.UpdatedById = UserBo.DefaultSuperUserId;
                EmailBo.AddData(password);
                bool success = GenerateMail();
                SaveEmail(ref trail);

                CreateTrail(
                    userBo.Id,
                    "Request Password"
                );

                if (!success)
                    return View(model);

                return RedirectToAction("Login");
            }
            AddResult(Result);

            return View(model);
        }

        // POST: Account/CheckUserSession
        [HttpPost]
        [AllowAnonymous]
        public JsonResult CheckUserSession()
        {
            bool show = false;
            bool refresh = false;
            bool logout = false;
            int secondsRemaining = 0;
            if (IsAuthenticated)
            {
                int sessionLength = int.Parse(Util.GetConfig("SessionLength"));
                int sessionNotice = int.Parse(Util.GetConfig("SessionLengthExpiryNotice"));

                long lastActivity = Session["lastActivity"] != null ? (long)Session["lastActivity"] : 0;
                DateTime lastActivityAt = new DateTime(lastActivity);
                DateTime now = DateTime.Now;
                TimeSpan difference = now - lastActivityAt;
                int sessionRemaining = sessionLength - sessionNotice;

                if (difference.TotalMinutes >= sessionLength)
                {
                    logout = true;
                }
                else if (difference.TotalMinutes >= sessionRemaining)
                {
                    if (!IsSessionTimeoutEnabled)
                    {
                        Session["lastActivity"] = DateTime.Now.Ticks;
                        refresh = true;
                    }
                    else
                    {
                        show = true;
                        secondsRemaining = (sessionLength * 60) - Convert.ToInt32(difference.TotalSeconds);
                    }
                }   
                else
                {
                    refresh = true;
                }
            }

            return Json(new { logout, show, secondsRemaining, refresh });
        }

        // POST: Account/RefreshUserSession
        [HttpPost]
        [AllowAnonymous]
        public JsonResult RefreshUserSession()
        {
            bool logout = true;
            int sessionLength = int.Parse(Util.GetConfig("SessionLength"));

            if (Session["lastActivity"] != null)
            {
                long ticks = long.Parse(Session["lastActivity"].ToString());
                DateTime lastActivityAt = new DateTime(ticks);
                DateTime now = DateTime.Now;
                TimeSpan difference = now - lastActivityAt;

                if (IsAuthenticated && difference.TotalMinutes < sessionLength)
                {
                    ObjectLockController.RefreshExpiryByUser(AuthUserId);
                    logout = false;
                }
            }

            return Json(new { logout });
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}