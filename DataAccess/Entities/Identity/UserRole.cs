using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.Identity
{
    [Table("UserRoles")]
    public class UserRole : IdentityUserRole<int> { }
}
