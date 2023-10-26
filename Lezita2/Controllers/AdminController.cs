﻿using Lezita2.Context;
using Lezita2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Lezita2.Controllers
{
    public class AdminController : Controller
    {
        LezitaDbContext _context = new LezitaDbContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetProducts()
        {
            ViewBag.Categories = _context.Categories.ToList();
            var products = _context.Products.ToList();
            if(products is null) return View();
            
            return View(products);
        }
        [HttpGet]
        public IActionResult AddProducts()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProducts(AddProductImage formProduct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.ToList();
                return View(formProduct);
            }
            var dictionary = "images";
            // Uzantıyı al (örneğin ".jpg")
            var fileExtension = Path.GetExtension(formProduct.Image.FileName);

            // Özgün bir dosya adı oluştur (guid + uzantı)
            var filename = $"{Guid.NewGuid()}{fileExtension}";

            // Dosyanın kaydedileceği tam yolu oluştur
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", dictionary, filename);

            // Dosyayı belirlenen yola kaydet
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formProduct.Image.CopyToAsync(stream);
            }
            //Nesne oluşturulup değerler atandı.
            Product product=new Product();
            product.Name = formProduct.Name;
            product.Description = formProduct.Description;
            product.CategoryId = formProduct.CategoryId;
            //Fotoğrafın url'i eklendi
            product.Image = $"/{dictionary}/{filename}";

            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("GetProducts");
        }
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }
        [HttpGet]
        public IActionResult AddCategories()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCategories(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("GetCategories");
        }
        [HttpPost]
        public IActionResult DeleteProducts(int id)
        {
            var product = _context.Products.FirstOrDefault(c => c.Id == id);
            if(product is not null) 
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            
            return RedirectToAction("GetProducts");
        }
        [HttpPost]
        public IActionResult DeleteCategories(int id)
        {
            var category=_context.Categories.FirstOrDefault(c => c.Id == id);
            if (category is not null)
            {
                var products = _context.Products.Where(c => c.CategoryId == id).ToList();
                if (products.Any())
                {
                    foreach (var product in products)
                    {
                        _context.Products.Remove(product);
                    }
                }
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
            
            return RedirectToAction("GetCategories");
        }
    }
}
