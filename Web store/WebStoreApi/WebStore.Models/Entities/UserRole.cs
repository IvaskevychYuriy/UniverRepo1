using Microsoft.AspNetCore.Identity;
using WebStore.Common.Contracts;

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
