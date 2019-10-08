using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products_Categories.Models;
using Microsoft.EntityFrameworkCore;

namespace Products_Categories.Controllers
{
    public class HomeController : Controller
    {
        private ProductContext dbContext;

        public HomeController(ProductContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Products()
        {
            List<Product> AllProducts = dbContext.Products.ToList();
            ViewBag.products = AllProducts;
            return View();
        }

        [HttpGet]
        [Route("/products/{productId}")]
        public IActionResult Product(int productId)
        {
            Product product = dbContext.Products.Include(u => u.Categories)
            .ThenInclude(c => c.Category)
            .FirstOrDefault(u => u.ProductId == productId);
            ViewBag.product = product;
            List<Category> AllCategories = dbContext.Categories.ToList();
            List<Category> usedCategories = new List<Category>();
            foreach(Association association in product.Categories) {
                usedCategories.Add(association.Category);
            }
            List<Category> unUsedCategories = new List<Category>();
            foreach(Category cat in AllCategories) {
                if(!usedCategories.Contains(cat)) {
                    unUsedCategories.Add(cat);
                }
            }
            ViewBag.categories = unUsedCategories;
            return View();
        }

        [HttpGet]
        [Route("/categories")]
        public IActionResult Categories()
        {
            List<Category> AllCategories = dbContext.Categories.ToList();
            ViewBag.categories = AllCategories;
            return View();
        }
        
        [HttpGet]
        [Route("/categories/{categoryId}")]
        public IActionResult Category(int categoryId)
        {
            Category category = dbContext.Categories.Include(u => u.Products)
            .ThenInclude(c => c.Product)
            .FirstOrDefault(u => u.CategoryId == categoryId);
            ViewBag.category = category;
            List<Product> AllProducts = dbContext.Products.ToList();
            List<Product> usedProducts = new List<Product>();
            foreach(Association association in category.Products) {
                usedProducts.Add(association.Product);
            }
            List<Product> unUsedProducts = new List<Product>();
            foreach(Product cat in AllProducts) {
                if(!usedProducts.Contains(cat)) {
                    unUsedProducts.Add(cat);
                }
            }
            ViewBag.products = unUsedProducts;
            return View();
        }

        [HttpPost]
        [Route("/product/new")] 
        public IActionResult CreateProduct(Product newProduct) {
            if(ModelState.IsValid){
                dbContext.Add(newProduct);
                dbContext.SaveChanges();
                return RedirectToAction("Products");
            }
            List<Product> AllProducts = dbContext.Products.ToList();
            ViewBag.products = AllProducts;
            return View("Products");
        }

        [HttpPost]
        [Route("/category/new")] 
        public IActionResult CreateCategory(Category newCategory) {
            if(ModelState.IsValid){
                dbContext.Add(newCategory);
                dbContext.SaveChanges();
                return RedirectToAction("Categories");
            }
            List<Category> AllCategories = dbContext.Categories.ToList();
            ViewBag.categories = AllCategories;
            return View("Categories");
        }

        [HttpPost]
        [Route("/add/category")] 
        public IActionResult AddCategory(Association newAssociation) {
            dbContext.Add(newAssociation);
            dbContext.SaveChanges();
            return Redirect($"/products/{newAssociation.ProductId}");
        }

        [HttpPost]
        [Route("/add/product")] 
        public IActionResult AddProduct(Association newAssociation) {
            dbContext.Add(newAssociation);
            dbContext.SaveChanges();
            return Redirect($"/categories/{newAssociation.CategoryId}");
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
