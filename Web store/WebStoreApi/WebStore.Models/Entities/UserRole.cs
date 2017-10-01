using Microsoft.AspNetCore.Identity;
using WebStore.Common.Contracts;

namespace WebStore.Models.Entities
{
    public class UserRole : IdentityRole<int>, IEntity<int>
    {
    }
}
