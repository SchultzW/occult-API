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
    [ApiController]
    [Route("Api/[controller]")]
    public class ProductController : ControllerBase
    {
        Regex imgUrlRegex = new Regex(@"^[;]");
        Regex emailRegEx = new Regex(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
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
        public ProductController(IProdRepos p, ICartRepo c)
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

        //public IActionResult Index()
        //{
        //    return View();
        //}
        //[HttpGet]
        //public ViewResult Browse()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult Browse(string searchText, string command)
        //{


        //    try
        //    {
        //        if (letterNumRegEx.IsMatch(searchText))
        //        {
        //            var results = (from p in pRepo.Products
        //                           where p.Title.Contains(searchText)
        //                           select p).ToList();
        //            return View("products", results);
        //        }

        //    }
        //    catch
        //    {
        //        return View("error");
        //    }

        //    return View("error");



        //}
        [HttpGet]
        public IActionResult GetProds()
        {
            try
            {
                var prods = pRepo.GetAllProducts();
                return Ok(prods);
            }
            catch
            {
                return NotFound();
            }
            

        }
        [HttpPatch("{tag}")]
        public IActionResult Products(string tag)
        {
            try
            {

                //pRepo.Prods.
                //FillRepo(tag);

                IEnumerable<Product> products = (from product in pRepo.Products
                                                 where product.Tag == tag
                                                 select product).ToList();

                //List < Product > prods = pRepo.Products.ToList();
                return Ok(products);
            }
            catch
            {
                return NotFound();
            }

        }
        [HttpGet("{ID}")]
        public IActionResult ProductDetails(int ID)
        {
            try
            {
              Product p = pRepo.GetProdByID(ID);


                return Ok(p);
            }
            catch
            {
                return NotFound();
            }

        }
        [HttpPost]
        public IActionResult AddProd([FromBody]ProductViewModel prod)
        {
            var i = prod.Price.ToString();
            try
            {
                if (prod != null)
                {
                    Product p = new Product
                    {
                        Title = prod.Title,
                        Description = prod.Description,
                        Price = int.Parse(i),
                        ImgPath = prod.ImgPath,
                        Tag = prod.Tag,
                        IsNew = prod.IsNew
                    };
                    pRepo.AddProd(p);
                    return Ok(p);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch
            {
                return NotFound();
            }

        }
        [HttpPut("{Id}")]
        public IActionResult Replace(string Id, [FromBody]ProductViewModel prod)
        {
            try
            {
                int id = int.Parse(Id);
                
                    Product p = new Product
                    {
                        Title = prod.Title.Trim(),
                        Description = prod.Description.Trim(),
                        Price = int.Parse(prod.Price.ToString()),
                        ImgPath = prod.ImgPath.Trim(),
                        Tag = prod.Tag,
                        IsNew = prod.IsNew
                    };

                    if (pRepo.UpdateProd(id, p) == true)
                    {
                        Console.WriteLine("Product Updated");
                    }


                    return Ok(p);
                
            }
            catch
            {
                return BadRequest();
            }
            

        }
        [HttpPatch("{id}")]
        public IActionResult UpdateProd(int id, [FromBody]PatchViewModel patchMod)
        {
            Product p = pRepo.GetProdByID(id);
            switch (patchMod.Path)
            {
                case "title":
                    p.Title = patchMod.Value;
                    break;
                case "description":
                    p.Description = patchMod.Value;
                    break;
                case "price":
                    p.Price = int.Parse(patchMod.Value);
                    break;
                case "imgPath":
                    p.ImgPath = patchMod.Value;
                    break;
                case "tag":
                    p.Tag = patchMod.Value;
                    break;
                case "isNew":
                    p.IsNew = bool.Parse(patchMod.Value);
                    break;
                default:
                    return BadRequest();


            }
            pRepo.UpdateProd(id, p);
            return Ok(p);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Product p = pRepo.GetProdByID(id);
            if (p != null)
            {
                pRepo.Delete(id);
                return NoContent(); //success nothing is found
            }
            else
                return NotFound();
        }



        //[HttpPost]
        //public IActionResult ProductDetails(string Name,string reviewText,string Command,int quantity, int id)
        //{

        //    try
        //    {


        //            Review review = new Review();
        //            review.Author = Name;
        //            review.ReviewText = reviewText;
        //            //review.ProductID = id;
        //            Product p = pRepo.GetProdByID(id);

        //            pRepo.AddReview(p, review);


        //            return View("Browse");






        //    }
        //    catch
        //    {
        //        return View("Error");
        //    }


        //}


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