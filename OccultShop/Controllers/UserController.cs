using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Midterm.Infrastructure;
using Midterm.Models;
using Midterm.Repos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OccultShop.Models;
using OccultShop.Repos;
using RestSharp;


namespace Midterm.Controllers
{
    [Authorize(Roles ="Admin, Customer")]
    public class UserController : Controller
    {
        ICartRepo cRepo;
        ICartItemRepo CIRepo;
        IProdRepos pRepo;
        IUserRepo uRepo;
        private UserManager<AppUser> userManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;

        public UserController(ICartRepo c, ICartItemRepo CI,IProdRepos p, IUserRepo u, UserManager<AppUser> usrMgr,
                IUserValidator<AppUser> userValid,
                IPasswordValidator<AppUser> passValid,
                IPasswordHasher<AppUser> passwordHash)
        {
            uRepo = u;
            CIRepo = CI;
            cRepo = c;
            pRepo = p;
            userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
        }
        //public UserController(ICartRepo c)
        //{
           
        //    cRepo = c;
        //}

        public ViewResult Profile()
        {
            return View();
        }
        public ViewResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(string command)
        {

            try
            {
                if (command == "drop")
                {
                    cRepo.ClearCart();
                    return View("index");
                }
                else
                {
                    var cartItems = (from item in CIRepo.CartItems
                                     select item).ToList();
                    return View("checkoutconfirm", cartItems);
                }
            }
            catch
            {
                return View("Error","home");

            }

            //if (command=="drop")
            //{
            //    cRepo.ClearCart();
            //    return View("index");
            //}
            //else
            //{
            //    var cartItems = (from item in CIRepo.CartItems
            //                     select item).ToList();
            //    return View("checkoutconfirm",cartItems);
            //}
        }
        [HttpGet]
        public ViewResult Cart()
        {
            //    //var products = (from item in CIRepo.CartItems
            //    //                             select item.CartProd).ToList();

            //    //// var quantity = (from item in CIRepo.CartItems
            //    //                 select item.Quantity).ToList();
            try
            {
                var cartItems = (from item in CIRepo.CartItems
                                 select item).ToList();


                return View(cartItems);
            }
            catch
            {
                return View("Error","home");

            }
        }




        [HttpPost]
        public IActionResult Cart(int ID,string command)
        {

            try
            {
                if (command == "checkout")
                {
                    var cartItems = (from item in CIRepo.CartItems
                                     select item).ToList();
                    return View("Checkoutconfirm");

                }
                else
                {
                    var deleteItem =
                   (from item in CIRepo.CartItems
                    where item.CartItemID == ID
                    select item).ToList();

                    Product p = deleteItem[0].CartProd;
                    CIRepo.Remove(deleteItem[0]);
                    return View("DeleteConfirm", p);
                }
            }
            catch
            {
                return View("Error", "home");

            }





        }
     
        [HttpGet]
        public ViewResult Horoscope()
        {
            try
            {
                return View();
            }
            catch
            {
                return View("Error");

            }
            
        }
        [HttpPost]
        public ActionResult Horoscope(string sign,string time)
        {

            try
            {
                MyHoroscope horoscope = new MyHoroscope();
                var client = new RestClient();

                IRestResponse response = client.Execute(new RestRequest("http://horoscope-api.herokuapp.com/horoscope/" + time + "/" + sign));

                string objects = JObject.Parse(response.Content).ToString();

                horoscope = JsonConvert.DeserializeObject<MyHoroscope>(objects);

                /*
                 * Account account = JsonConvert.DeserializeObject<Account>(json);
                 * need to create a model for response from API so i can pass it 
                 * into view
                 * 
                 * need to update view page so user can choose sign
                 * 
                 * need to hook up routes to horoscope page.
                 * 
                 *totalCreditsRemoved = (String)objects.Value["totalCreditsRemoved"];
                    invalidReceivers = (String)objects.Value["invalidReceivers"];
                    ids = (String)objects.Value["ids"];
                    validReceivers = (String)objects.Value["validReceivers"]; 
                 * 
                 * 
                 */

                return View("HoroscopeResult", horoscope);
            }
            catch
            {
                return View("Error", "home");

            }


        }

        [HttpGet]
        public ActionResult UserSignUpInfo()
        {
            try
            {
                return View();
            }
            catch
            {
                return View("Error", "home");

            }
           
        }
        public ActionResult UserSignUpInfo(string password1, string password,string firstName, 
                                            string lastName, string address, string city, string state,string zip,string email )
        {
            try
            {
                if (ModelState.GetValidationState(nameof(address)) == ModelValidationState.Valid &&
            //password.Equals(password1) &&
            ModelState.GetValidationState(nameof(firstName)) == ModelValidationState.Valid &&
             ModelState.GetValidationState(nameof(lastName)) == ModelValidationState.Valid &&
            ModelState.GetValidationState(nameof(city)) == ModelValidationState.Valid &&
            ModelState.GetValidationState(nameof(zip)) == ModelValidationState.Valid &&
            ModelState.GetValidationState(nameof(state)) == ModelValidationState.Valid &&
            ModelState.GetValidationState(nameof(email)) == ModelValidationState.Valid)
                {
                    AppUser u = new AppUser();
                    u.FirstName = firstName.Trim();
                    u.LastName = lastName.Trim();
                    u.Password = password.Trim();
                    u.Address = address.Trim();
                    //u.Email = email.Trim();
                    u.Zip = zip.Trim();
                    u.State = state.Trim();
                    u.City = city.Trim();

                    uRepo.AddUser(u);
                    return View("Profile");


                }

                return View("UserSignUpInfo");
            }
            catch
            {
                return View("Error", "home");
            }
           
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> UserSignUpInfo(AppUser model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName=model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    Zip = model.Zip,
                    State = model.State,
                    City = model.City

                };
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("AdminUser");
                }
                else
                {
                    foreach (IdentityError e in result.Errors)
                    {
                        ModelState.AddModelError(" ", e.Description);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Admin(String title,string description,int price, string ImgPath,string tag,bool isNew)
        {
            try
            {
                if (ModelState.GetValidationState(nameof(title)) == ModelValidationState.Valid &&
              ModelState.GetValidationState(nameof(description)) == ModelValidationState.Valid &&
              ModelState.GetValidationState(nameof(price)) == ModelValidationState.Valid &&
              ModelState.GetValidationState(nameof(ImgPath)) == ModelValidationState.Valid &&
              ModelState.GetValidationState(nameof(tag)) == ModelValidationState.Valid &&
              ModelState.GetValidationState(nameof(isNew)) == ModelValidationState.Valid)
                {
                    Product p = new Product();
                    p.Title = title.Trim();
                    p.Description = description.Trim();
                    p.Price = price;
                    p.ImgPath = ImgPath.Trim();
                    p.Tag = tag;
                    p.IsNew = isNew;
                    //pRepo.AddProd(p);
                }


                return View("Admin");
            }
            catch
            {
                return View("Error", "home");
            }


        }
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