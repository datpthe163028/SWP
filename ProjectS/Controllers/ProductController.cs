﻿using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    public class ProductController : Controller
    {

        private readonly ILogger<ProductController> _logger;
        private readonly ShopContext _shopContext;
        public ProductController(ILogger<ProductController> logger, ShopContext ct)
        {
            _shopContext = ct;
            _logger = logger;
        }


        public IActionResult Index(int id, bool gender)
        {
            List<SubCategory> subCategories = new List<SubCategory>();
            var list = (from p in _shopContext.Products
                       where p.SubCategoryID == id && p.typeGender != gender
                       select p).ToList();
            var subCate = (from c in _shopContext.SubCategory
                          where c.SubCategoryId == id
                          select c).FirstOrDefault();
            var cateGory = (from c in _shopContext.Categories
                           where c.CategoryId == subCate.CateogoryId
                           select c).First();
            if (cateGory != null)
            {
                var e = _shopContext.Entry(cateGory);
                e.Collection(c => c.SubCategories).Load();
                subCategories  = cateGory.SubCategories;
            }
            subCategories.Sort( (x,y) => x.SubCategoryId.CompareTo(y.SubCategoryId) );
           
            ViewData["listSubCate"] = subCategories;
            ViewData["subCate"] = subCate;
            ViewData["gender"] = gender;


            return View(list);
        }
    }
}