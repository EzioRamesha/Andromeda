using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.Identity
{
    [Table("UserClaims")]
    public class UserClaim : IdentityUserClaim<int> { }
}
