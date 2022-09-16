using Codecool.CodecoolShop.Daos.Implementations.Database;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Codecool.CodecoolShop.Controllers
{
    public class AutentificationController : BaseController
    {
        public AutentificationController(ILogger<BaseController> logger, IConfiguration config) : base(logger, config)
        {
        }
        public IActionResult Login(User user)
        {
            if (string.IsNullOrEmpty(user.Email) && string.IsNullOrEmpty(user.Password))
            {
                ViewBag.ErrorMsg = "Email and password are empty";
                return View("Login");
            }
            else
            {
                if (new UserDaoDatabase(_config).UserExist(user.Email, user.Password))
                {
                    HttpContext.Session.SetString("UserName", user.Email);
                    return RedirectToAction(actionName: "Index", controllerName: "Product");
                }
            }
            return View("Login");
        }
        public IActionResult Register(User user)
        {
            if (string.IsNullOrEmpty(user.Email) && string.IsNullOrEmpty(user.Password))
            {
                return View("Register");
            }
            new UserDaoDatabase(_config).Add(user);
            HttpContext.Session.SetString("UserName", user.Email);
            return RedirectToAction(actionName: "Index", controllerName: "Product");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserName");
            return RedirectToAction(actionName: "Index", controllerName: "Product");
        }
    }
}
