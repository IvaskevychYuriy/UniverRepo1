using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using WebStore.Common.Contracts;

namespace WebStore.Models.Entities
{
    public class User : IdentityUser<int>, IEntity<int>
    {
        public List<Order> Orders { get; set; }
    }
}
