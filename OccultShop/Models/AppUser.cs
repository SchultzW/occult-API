using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Midterm.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Midterm.Models
{
    public class AppUser:IdentityUser
    {
        private static List<Cart> cart = new List<Cart>();
        public static List<Cart> Cart { get { return cart; } }

        [Required(ErrorMessage = "Please enter a first name")]
        [RegularExpression(@"^(?<firstchar>(?=[A-Za-z]))((?<alphachars>[A-Za-z])|(?<specialchars>[A-Za-z]['-](?=[A-Za-z]))|(?<spaces> (?=[A-Za-z])))*$", ErrorMessage = "Please enter a first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a last name")]
        [RegularExpression(@"^(?<firstchar>(?=[A-Za-z]))((?<alphachars>[A-Za-z])|(?<specialchars>[A-Za-z]['-](?=[A-Za-z]))|(?<spaces> (?=[A-Za-z])))*$",ErrorMessage = "Please enter a last name")]
        public string LastName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter a password that is at least 8 characters")]
        [StringLength(60, MinimumLength = 8)]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Please enter an email")]
        //[EmailAddress]
        //public string Email { get; set; }

        [Required(ErrorMessage = "Please enter an address")]
        [StringLength(60, MinimumLength = 3)]
        public string Address { get; set; }

        [Required(ErrorMessage ="Please enter a valid zip code")]
        [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "Please enter a valid zip code")]
        [DataType(DataType.PostalCode)]
        public string Zip { get; set; }

        [Required]
        public string State { get; set; }

        [RegularExpression(@"^(?<firstchar>(?=[A-Za-z]))((?<alphachars>[A-Za-z])|(?<specialchars>[A-Za-z]['-](?=[A-Za-z]))|(?<spaces> (?=[A-Za-z])))*$", ErrorMessage = "Please enter a City")]
        [Required(ErrorMessage = "Please enter a City")]
        public string City { get; set; }
        public bool IsAdmin { get; set; }

       
    }
}
