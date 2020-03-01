using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Midterm.Models
{
    public class Product
    {
        private List<Review> reviews = new List<Review>();
        public ICollection<Review> Reviews { get { return reviews; } }

        public int ProductId { get; set; }

        //[Required(ErrorMessage = "Please enter a title")]

        public string Title { get; set; }

        //[Required(ErrorMessage = "Please enter a description that is less than 250 characters")]

        public string Description { get; set; }

        //[Required(ErrorMessage = "Please enter a price")]
        //[RegularExpression(@"^[1-9]\d{0,7}(?:\.\d{1,4})?$", ErrorMessage = "The price must be a positive number")]
        public int Price { get; set; }

        public string Tag { get; set; }

        public string ImgPath { get; set; }

        public bool IsNew { get; set; }









    }
}
