using Client.Contracts.Data;
using Client.Utilities.Handlers;
using Client.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Client.Controllers
{
    [Controller]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var result = await _accountRepository.Login(loginVM);
            if (result == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (result.Code == 404)
            {
                TempData["Error"] = $"{result.Message}!";
                return View();
            }
            else if (result.Code == 200)
            {
                HttpContext.Session.SetString("JWToken", result.Data);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromForm] RegisterVM registerVM)
        {
            var result = await _accountRepository.Register(registerVM);
            if (result.Errors != null)
            {
                if (result.Errors is Dictionary<string, List<string>> errorDictionary)
                {
                    foreach (var key in errorDictionary.Keys)
                    {
                        foreach (var errorMessage in errorDictionary[key])
                        {
                            ModelState.AddModelError(key, errorMessage);
                        }
                    }
                }
                var components = new ComponentHandlers
                {
                    Footer = false,
                    SideBar = false,
                    Navbar = false,
                };
                ViewBag.Components = components;
                string errorsJson = JsonConvert.SerializeObject(result.Errors);
                TempData["Error"] = errorsJson;

                return View("Register");
            }
            return RedirectToAction("Login");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var account = await _accountRepository.Get();
            var components = new ComponentHandlers
            {
                Footer = false,
                SideBar = true,
                Navbar = true,
            };
            var profile = new GetProfileVM
            {
                Email = account.Data.Email,
                PhoneNumber = account.Data.PhoneNumber,
                Name = account.Data.Name,
            };

            ViewBag.Components = components;
            return View("Profile", profile);
        }

        [HttpPost]
        public async Task<IActionResult> Profile([FromForm] GetProfileVM updateProfileVM)
        {
            var result = await _accountRepository.Update(updateProfileVM);
            if (result.Errors != null)
            {
                if (result.Errors is Dictionary<string, List<string>> errorDictionary)
                {
                    foreach (var key in errorDictionary.Keys)
                    {
                        foreach (var errorMessage in errorDictionary[key])
                        {
                            ModelState.AddModelError(key, errorMessage);
                        }
                    }
                }
                var components = new ComponentHandlers
                {
                    Footer = false,
                    SideBar = true,
                    Navbar = true,
                };
                ViewBag.Components = components;
                string errorsJson = JsonConvert.SerializeObject(result.Errors);
                TempData["Error"] = errorsJson;

                return View("Profile");
            }
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
