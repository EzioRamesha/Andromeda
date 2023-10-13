using BusinessObject.Identity;
using DataAccess.Entities.Identity;
using DataAccess.EntityFramework;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Threading.Tasks;

namespace DataAccess.Identity
{
    public class AppUserStore : UserStore<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        private readonly AppDbContext _db;

        public AppUserStore() : base(new AppDbContext())
        {

        }

        public AppUserStore(AppDbContext context) : base(context)
        {
            _db = context;
        }

        public override Task<User> FindByNameAsync(string userName)
        {
            return _db.Users
                .Include(u => u.Logins).Include(u => u.Roles).Include(u => u.Claims)
                .FirstOrDefaultAsync(u => u.UserName == userName && u.Status != UserBo.StatusDelete);
        }
        
        public override Task<User> FindByEmailAsync(string email)
        {
            return _db.Users
                .Include(u => u.Logins).Include(u => u.Roles).Include(u => u.Claims)
                .FirstOrDefaultAsync(u => u.Email == email && u.Status != UserBo.StatusDelete);
        }
    }
}
