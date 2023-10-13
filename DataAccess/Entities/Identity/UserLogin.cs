using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.Identity
{
    [Table("UserLogins")]
    public class UserLogin : IdentityUserLogin<int> { }
}
