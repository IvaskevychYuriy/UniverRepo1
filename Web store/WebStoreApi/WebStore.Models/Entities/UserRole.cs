using Microsoft.AspNetCore.Identity;
using WebStore.Models.Contracts;

namespace WebStore.Models.Entities
{
    public class UserRole : IdentityRole<int>, IEntity<int>
    {
        public UserRole() : base()
        {
        }

        public UserRole(string roleName) : base(roleName)
        {
        }
    }
}
