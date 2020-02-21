using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Midterm.Models;
using Midterm.Repos;
using OccultShop.Models;

namespace OccultShop.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        Regex imgUrlRegex = new Regex(@"^[;]");
        Regex emailRegEx = new Regex(@"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
        Regex letterNumRegEx = new Regex(@"^[a-zA-Z0-9]+$");
        private UserManager<AppUser> userManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;
        IProdRepos pRepo;
        private SignInManager<AppUser> signInManager;



        public AdminController(UserManager<AppUser> usrMgr,
                IUserValidator<AppUser> userValid,
                IPasswordValidator<AppUser> passValid,
                IPasswordHasher<AppUser> passwordHash,IProdRepos p, SignInManager<AppUser>signInMgr)
        {
            signInManager = signInMgr;
            pRepo = p;
            userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
        }


        [HttpGet]
        public ViewResult Admin()
        {
            return View();
        }
        [HttpGet]
        public ViewResult AdminProd()
        {

            IEnumerable<Product> products = (from product in pRepo.Products
                                             select product).ToList();
            return View(products);
        }
        [HttpGet]
        public ViewResult AdminUser()
        {
            return View(userManager.Users);
        }
        [HttpGet]
        public ViewResult CreateUser()
        {
            return View();
        }
        [HttpGet]
        public ViewResult AddProd()
        {
            return View();
        }
      
        [HttpPost]
        public ViewResult AdminProd(string id)
        {
            Product p = new Product();
            try
            {
                if(letterNumRegEx.IsMatch(id))
                {
                    IEnumerable<Product> products = (from product in pRepo.Products
                                                     where product.ProductId == int.Parse(id)
                                                     select product).ToList();
                    p = products.First();
                }
               
            }
            catch
            {
                return View("Error");
            }
            

            return View(p);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(AppUser model)
        {

            if(ModelState.IsValid)
            {
                if(emailRegEx.IsMatch(model.Email) &&
                   letterNumRegEx.IsMatch(model.FirstName)&&
                    letterNumRegEx.IsMatch(model.LastName)&&
                     letterNumRegEx.IsMatch(model.Address)&&
                      letterNumRegEx.IsMatch(model.Zip)&&
                        letterNumRegEx.IsMatch(model.State)&&
                         letterNumRegEx.IsMatch(model.City))
                {
                    AppUser user = new AppUser
                    {
                        UserName = model.Email,
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
               
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user !=null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("AdminUser");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError(" ", "User not found.");
            }
            return View("AdminUser", userManager.Users);
        }

        
        public async Task<IActionResult> EditUser(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("AdminUser");
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(string id, string email,string password,string firstName, string lastName, string address,
                                                string city, string zip, string state)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                //user.Email = email;
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Address = address;
                user.City = city;
                user.Zip = zip;
                user.State = state;

                IdentityResult validEmail
                    = await userValidator.ValidateAsync(userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager,
                        user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user,
                            password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }
                if ((validEmail.Succeeded && validPass == null)
                        || (validEmail.Succeeded
                        && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("AdminUser");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }

        /// <summary>
        /// For Prods
        /// </summary>
        [HttpPost]
        public IActionResult AddProd(string title, string description, string price, string imgPath, string tag, bool isNew)
        {
            try
            {
                if (letterNumRegEx.IsMatch(title) &&
                    letterNumRegEx.IsMatch(description) &&
                    letterNumRegEx.IsMatch(tag) &&
                    !imgUrlRegex.IsMatch(imgPath))
                {
                    Product p = new Product
                    {
                        Title = title.Trim(),
                        Description = description.Trim(),
                        Price = int.Parse(price),
                        ImgPath = imgPath.Trim(),
                        Tag = tag,
                        IsNew = isNew

                    };
                    pRepo.AddProd(p);
                    return View("AdminProd");
                }
                else
                    return View("Error");
            }
            catch
            {
                return View("Error");
            }
        
        }
        [HttpGet]
        public IActionResult EditProd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult EditProd(string title, string description, string price, string imgPath, string tag, bool isNew, int productId)
        {
            try
            {
                if (letterNumRegEx.IsMatch(title) &&
                    letterNumRegEx.IsMatch(description) &&
                    letterNumRegEx.IsMatch(tag) &&
                    !imgUrlRegex.IsMatch(imgPath))
                {
                    Product p = new Product
                    {
                        Title = title.Trim(),
                        Description = description.Trim(),
                        Price = int.Parse(price),
                        ImgPath = imgPath.Trim(),
                        Tag = tag,
                        IsNew = isNew
                    };

                    if (pRepo.UpdateProd(productId, p) == true)
                    {
                        Console.WriteLine("Product Updated");
                    }


                    return View("AdminProd");
                }
            }
            catch
            {
                return View("Error");
            }
            return View("Error");

        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {

            ViewBag.returnUrl = returnUrl; return View();

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LogInViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    var result = await signInManager.PasswordSignInAsync(user,model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LogInViewModel.Email),"Invalid user or password");

            }
            return View(model);
        }




        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        [AllowAnonymous]
        public async Task<RedirectToActionResult> Logout()
        {

            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");

        }



    }
}