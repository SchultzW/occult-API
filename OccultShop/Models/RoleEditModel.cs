using Microsoft.AspNetCore.Identity;
using Midterm.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OccultShop.Models
{
    public class RoleEditModel
    {
        public class RoleEditViewModel
        {
            public IdentityRole Role { get; set; }
            public IEnumerable<AppUser> Members { get; set; }
            public IEnumerable<AppUser> NonMembers { get; set; }
        }

        public class RoleModificationModel
        {
            [Required]
            public string RoleName { get; set; }
            public string RoleId { get; set; }
            public string[] IdsToAdd { get; set; }
            public string[] IdsToDelete { get; set; }
        }
    }
}
