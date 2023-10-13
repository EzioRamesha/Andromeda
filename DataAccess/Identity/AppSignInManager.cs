using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Shared;
using Shared.DirectoryServices;
using System;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataAccess.Identity
{
    // Configure the application sign-in manager which is used in this application.
    public class AppSignInManager : SignInManager<User, int>
    {
        string _error;
        bool _isAdConnectionIssue;

        public AppSignInManager(AppUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
            _isAdConnectionIssue = false;
        }

        public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            if (userName == null)
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var user = await UserManager.FindByNameAsync(userName);
            if (user == null || user.Status == UserBo.StatusSuspend)
                return SignInStatus.Failure;

            bool locked = await UserManager.IsLockedOutAsync(user.Id);
            if (await UserManager.IsLockedOutAsync(user.Id))
            {
                return SignInStatus.LockedOut;
            }

            bool adEnabled = Boolean.Parse(Util.GetConfig("ActiveDirectoryEnabled"));
            // If login is Password
            if (user.LoginMethod == 1 || !adEnabled)
            {
                if (user.PasswordHash != null)
                {
                    return await base.PasswordSignInAsync(userName, password, isPersistent, shouldLockout);
                }
                return SignInStatus.Failure;
            }
            
            // LDAP
            UserPrincipal userPrincipal = null;

            bool isAuthenticated;
            try
            {
                DirectoryService directoryService = new DirectoryService();

                isAuthenticated = directoryService.Context.ValidateCredentials(userName, password, ContextOptions.Negotiate);
                if (isAuthenticated)
                {
                    userPrincipal = UserPrincipal.FindByIdentity(directoryService.Context, IdentityType.SamAccountName, userName);
                }
            }
            catch (Exception e)
            {
                _error = e.Message;
                _isAdConnectionIssue = true;
                isAuthenticated = false;
                userPrincipal = null;
            }

            if (!isAuthenticated || userPrincipal == null)
            {
                if (UserManager.SupportsUserLockout && shouldLockout && !_isAdConnectionIssue)
                {
                    // If lockout is requested, increment access failed count which might lock out the user
                    await UserManager.AccessFailedAsync(user.Id);
                    if (await UserManager.IsLockedOutAsync(user.Id))
                    {
                        return SignInStatus.LockedOut;
                    }
                }
                return SignInStatus.Failure;
            }

            await SignInAsync(user, isPersistent, false);
            await UserManager.ResetAccessFailedCountAsync(user.Id);
            return SignInStatus.Success;
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((AppUserManager)UserManager);
        }

        public static AppSignInManager Create(IdentityFactoryOptions<AppSignInManager> options, IOwinContext context)
        {
            return new AppSignInManager(context.GetUserManager<AppUserManager>(), context.Authentication);
        }

        public string getError()
        {
            if (string.IsNullOrEmpty(_error))
                return null;

            if (!_isAdConnectionIssue)
                return _error;

            return string.Format("Active Directory Connection Issue: {0}", _error);
        }
    }
}
