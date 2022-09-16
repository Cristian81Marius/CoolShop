using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Daos.Implementations;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System;
using Codecool.CodecoolShop.Daos.Implementations.Database;
using Microsoft.AspNetCore.Http;



namespace Codecool.CodecoolShop.Controllers
{
    public class CartController : BaseController
    {
        public CartController(ILogger<BaseController> logger, IConfiguration config) : base(logger, config)
        {
        }
        public IActionResult Cart()
        {
            if (_config == null)
            {
                var cart = CartDaoMemory.GetInstance().GetAll();
                return View(cart.ToList());
            }
            else
            {
                int userId = new UserDaoDatabase(_config).GetId(HttpContext.Session.GetString("UserName"));
                var cart = new CartDaoDatabase(_config).GetByUserId(userId);
                return View(cart.ToList());
            }
        }

        public IActionResult AddToCart(int id)
        {
            if (_config == null)
            {
                var product = ProductDaoMemory.GetInstance().Get(id);
                CartDaoMemory.GetInstance().Add(new Cart() { product = product, amount = 1 });
            }
            else
            {
                var userId = new UserDaoDatabase(_config).GetId(HttpContext.Session.GetString("UserName"));
                var product = new ProductDaoDatabase(_config).Get(id);
                if (new CartDaoDatabase(_config).Get(id).amount == 0)
                {
                    new CartDaoDatabase(_config).Add(new Cart() { product = product, amount = 1, userId = userId });
                }
                else
                {
                    new CartDaoDatabase(_config).Update(product.Id, userId);
                }
            }
            return RedirectToAction(actionName: "Index", controllerName: "Product", new { cat = 0 });
        }
        public IActionResult DeleteFromCart(int id)
        {
            if (_config == null)
            {
                CartDaoMemory.GetInstance().Remove(id);
            }
            else
            {
                new CartDaoDatabase(_config).Remove(id);
            }
            return RedirectToAction(actionName: "Cart", controllerName: "Cart");
        }
        public IActionResult PaymentComplete()
        {
            //validate informatiom
            if (_config == null)
            {
                var shipped = CartDaoMemory.GetInstance().GetAll();
                foreach (Cart cart in shipped)
                {
                    ProductDaoMemory.GetInstance().Remove(cart.product.Id);

                }
                CartDaoMemory.GetInstance().RemoveAll();
            }
            else
            {
                var shipped = new CartDaoDatabase(_config).GetAll();
                new CartDaoDatabase(_config).RemoveAll();
                foreach (Cart cart in shipped)
                {
                    Product product = new ProductDaoDatabase(_config).Get(cart.product.Id);
                    product.Amount = product.Amount > cart.amount ? product.Amount - cart.amount : 0;
                    new ProductDaoDatabase(_config).Update(product);
                    //new ProductDaoDatabase(_config).Remove(cart.product.Id);
                }
            }
            return View("PaymentComplete");
        }
        public IActionResult BuyProducts()
        {
            return View("Payment");
        }
    }
}
