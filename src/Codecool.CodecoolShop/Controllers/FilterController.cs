using Codecool.CodecoolShop.Daos.Implementations;
using Codecool.CodecoolShop.Daos.Implementations.Database;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Codecool.CodecoolShop.Controllers
{
    public class FilterController : BaseController
    {
        public FilterController(ILogger<BaseController> logger, IConfiguration config) : base(logger, config)
        {
        }
        [HttpGet]
        [Route("filter/category/{category}")]
        public PartialViewResult FilterCategory(int category)
        {
            //save data in frontend sesion, local storage, 
            if (_config == null)
            {
                var filterList = ProductService.GetProductsForCategory(category);
                if (category == 0)
                {
                    filterList = ProductDaoMemory.GetInstance().GetAll();
                }
                return PartialView("CreateCards", filterList.ToList());
            }
            else
            {
                if (category == 0)
                {
                    var filterList = new ProductDaoDatabase(_config).GetAll();
                    return PartialView("CreateCards", filterList.ToList());
                }
                else
                {

                    var filterList = new ProductDaoDatabase(_config).GetBy(new ProductCategoryDaoDatabase(_config).Get(category));
                    return PartialView("CreateCards", filterList.ToList());
                }

            }
        }
        [Route("filter/supplier/{supplier}")]

        public PartialViewResult FilterSupplier(int supplier)
        {
            //save data in frontend sesion, local storage, 
            if (_config == null)
            {
                var filterList = ProductService.GetProductsForSupplier(supplier);
                if (supplier == 0)
                {
                    filterList = ProductDaoMemory.GetInstance().GetAll();
                }
                return PartialView("CreateCards", filterList.ToList());
            }
            else
            {
                if (supplier == 0)
                {
                    var filterList = new ProductDaoDatabase(_config).GetAll();
                    return PartialView("CreateCards", filterList.ToList());
                }
                else
                {
                    var filterList = new ProductDaoDatabase(_config).GetBy(new SupplierDaoDatabase(_config).Get(supplier));
                    return PartialView("CreateCards", filterList.ToList());
                }
            }


        }
    }
}
