using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OccultShop.Models
{
    public class ProductViewModel
    {


        public int ProductId { get; set; }

    

        public string Title { get; set; }

       

        public string Description { get; set; }

 
        public int Price { get; set; }

        public string Tag { get; set; }

        public string ImgPath { get; set; }

        public bool IsNew { get; set; }
    }
}
