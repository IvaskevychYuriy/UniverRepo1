using Microsoft.AspNetCore.Identity;
using WebStore.Common.Contracts;

namespace WebStore.Models.Entities
{
    public class User : IdentityUser<int>, IEntity<int>
    {
    }
}
