using Midterm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OccultShop.Repos
{
    public class FakeUserRepo : IUserRepo
    {
        public void AddUser(User u)
        {
            throw new NotImplementedException();
        }
    }
}
