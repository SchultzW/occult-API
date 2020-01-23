using Midterm.Infrastructure;
using Midterm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OccultShop.Repos
{
    public interface IUserRepo
    {
         

        void AddUser(AppUser u);
     
    }
}
