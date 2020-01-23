using Midterm.Infrastructure;
using Midterm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OccultShop.Repos
{
    public class UserRepo:IUserRepo
    {
        private AppDbContext context;

        public void AddUser(AppUser u)
        {
            context.Add(u);
            context.SaveChanges();
        }
    }
}
