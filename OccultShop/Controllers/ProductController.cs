using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Midterm.Models;
using Midterm.Infrastructure;
using static Midterm.Models.Cart;
using Midterm.Repos;
using OccultShop.Repos;
using OccultShop.Models;
using System.Text.RegularExpressions;

namespace Midterm.Controllers
{
    public class ProductController : Controller
    {
        Regex letterNumRegEx = new Regex(@"^[a-zA-Z0-9]+$");
        //ICartItemRepo CIRepo;
        IProdRepos pRepo;
        ICartRepo cRepo;
        Product p = new Product();
        Product p1 = new Product();
        Product p2 = new Product();
        Product p3 = new Product();
        Product p4 = new Product();

        //icartrepo 
        //public ProductController(IProdRepos r,ICartItemRepo CI )
        //{
        //    pRepo = r;
        //    CIRepo = CI;
        //}
        public ProductController(IProdRepos p,ICartRepo c)
        {
            pRepo = p;
            cRepo = c;

        }
        //public ProductController(IProdRepos p)
        //{
        //    pRepo = p;
        //}

        List<Product> allProds = new List<Product>();
        List<Product> products = new List<Product>();
        List<CartItem> cart = new List<CartItem>();
       
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ViewResult Browse()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Browse(string searchText,string command)
        {
           
            
                try
                {
                    if (letterNumRegEx.IsMatch(searchText))
                    {
                    var results = (from p in pRepo.Products
                                   where p.Title.Contains(searchText)
                                   select p).ToList();
                    return View("products", results);
                }
                   
                }
                catch
                {
                    return View("error");
                }
              
                return View("error");



        }
        [HttpGet]
        public ViewResult Products(string tag)
        {
            try
            {

                //pRepo.Prods.
                //FillRepo(tag);

                IEnumerable<Product> products = (from product in pRepo.Products
                                                 where product.Tag == tag
                                                 select product).ToList();

                //List < Product > prods = pRepo.Products.ToList();
                return View(products);
            }
            catch 
            {
                return View("Error");
            }

        }
        [HttpGet]
        public ViewResult ProductDetails(int ID)
        {
            try
            {
                Product p = pRepo.GetProdByID(ID);


                return View(p);
            }
            catch
            {
                return View("Error");
            }

        }
    
        [HttpPost]
        public IActionResult ProductDetails(string Name,string reviewText,string Command,int quantity, int id)
        {

            try
            {
                if (Command == "AddReview")
                {
                    Review review = new Review();
                    review.Author = Name;
                    review.ReviewText = reviewText;
                    //review.ProductID = id;
                    Product p = pRepo.GetProdByID(id);

                    pRepo.AddReview(p, review);


                    return View("Browse");
                }
                else if (Command == "AddCart")
                {
                    Product p;
                    CartItem item = new CartItem();
                    p = pRepo.GetProdByID(id);
                    item.CartProd = p;
                    item.Quantity = quantity;
                    cRepo.AddToCart(item);
                    cart.Add(item);
                    return View("PurchaseConfirm", p);
                }
                else
                    return View("Error");


            }
            catch
            {
                return View("Error");
            }
          

        }
       

        /// <summary>
        /// fills repo with prods that match the tag
        /// </summary>
        /// <param name="tag"></param>
        public void FillRepo(string tag)
        {
            foreach(Product p in allProds)
            {
                if(p.Tag==tag)
                {
                    pRepo.AddProd(p);
                }
            }

        }

        /// <summary>
        /// test data so product doesnt throw null
        /// </summary>
       

        /// <summary>
        /// session stuff doenst work at the moment >_<
        /// </summary>
        /// <returns></returns>
        private List<CartItem> GetCart()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            return cart;
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }


    }
}