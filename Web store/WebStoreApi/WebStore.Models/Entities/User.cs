using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using WebStore.Common.Contracts;

namespace WebStore.Models.Entities
{
    public class User : IdentityUser<int>, IEntity<int>
    {
        public User() : base()
        {
            Init();
        }

        public User(string userName) : base(userName)
        {
            Init();
        }

        private void Init()
        {
            Orders = new HashSet<Order>();
        }

        public ICollection<Order> Orders { get; set; }
    }
}
