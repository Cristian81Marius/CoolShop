using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Daos.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Microsoft.Extensions.Configuration;
using Codecool.CodecoolShop.Daos.Implementations.Database;
using Microsoft.AspNetCore.Http;

namespace Codecool.CodecoolShop.Controllers
{
    public class ProductController : BaseController
    {
        public ProductController(ILogger<ProductController> logger, IConfiguration config) : base(logger, config)
        {
        }

        public IActionResult Index()
        {
            var products = new ProductDaoDatabase(_config).GetAll();
            var category = new ProductCategoryDaoDatabase(_config).GetAll();
            var supplier = new SupplierDaoDatabase(_config).GetAll();
            if (_config == null)
            {
                category = ProductCategoryDaoMemory.GetInstance().GetAll();
                products = ProductDaoMemory.GetInstance().GetAll();
                supplier = SupplierDaoMemory.GetInstance().GetAll();
            }
            if (HttpContext.Session.GetString("UserName") != null)
            {
                var userId = new UserDaoDatabase(_config).GetId(HttpContext.Session.GetString("UserName"));
                var carts = new CartDaoDatabase(_config).GetByUserId(userId);
                foreach (Product product in products) {
                    product.IsAvailable = carts.Where(x => x.product.Id == product.Id).Select(x => x.amount).FirstOrDefault() < product.Amount;
                        }
            }
            ViewData["category"] = category;
            ViewData["supplier"] = supplier;
            return View(products.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
