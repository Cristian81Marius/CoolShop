using Codecool.CodecoolShop.Daos.Implementations;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Codecool.CodecoolShop.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ILogger<BaseController> Logger;
        public ProductService ProductService { get; set; }

        protected IConfiguration _config;
        CommonHelper _helper;

        protected BaseController(ILogger<BaseController> logger, IConfiguration config)
        {
            _config = config;
            _helper = new CommonHelper(_config); 
                
            Logger = logger;
            ProductService = new ProductService(
                ProductDaoMemory.GetInstance(),
                ProductCategoryDaoMemory.GetInstance());
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); ;
        }
    }
}
