using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Shared;
using System;

namespace DataAccess.Identity
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class AppUserManager : UserManager<User, int>
    {
        public AppUserManager(AppUserStore store) : base(store)
        {
        }

        public AppUserManager(IUserStore<User, int> store) : base(store)
        {
        }

        public static AppUserManager Default()
        {
            return Create(null, null);
        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var db = new AppDbContext();
            if (context != null)
            {
                db = context.Get<AppDbContext>();
            }

            var manager = new AppUserManager(new AppUserStore(db));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = Boolean.Parse(Util.GetConfig("AllowOnlyAlphanumericUserNames")),
                RequireUniqueEmail = Boolean.Parse(Util.GetConfig("RequireUniqueEmail")),
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = Int32.Parse(Util.GetConfig("PasswordMinLength")),
                RequireNonLetterOrDigit = Boolean.Parse(Util.GetConfig("RequireNonLetterOrDigit")),
                RequireDigit = Boolean.Parse(Util.GetConfig("PasswordRequireDigit")),
                RequireLowercase = Boolean.Parse(Util.GetConfig("PasswordRequireLowercase")),
                RequireUppercase = Boolean.Parse(Util.GetConfig("PasswordRequireUppercase")),
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = Boolean.Parse(Util.GetConfig("UserLockoutEnabledByDefault"));
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = Int32.Parse(Util.GetConfig("MaxFailedAccessAttemptsBeforeLockout"));

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User, int>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();

            if (options != null)
            {
                var dataProtectionProvider = options.DataProtectionProvider;
                if (dataProtectionProvider != null)
                {
                    manager.UserTokenProvider = new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("ASP.NET Identity"));
                }
            }
            return manager;
        }
    }
}
