using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities.Identity
{
    [Table("Roles")]
    public class Role : IdentityRole<int, UserRole>
    {
        public Role() { }

        public Role(string name) { Name = name; }
    }
}
