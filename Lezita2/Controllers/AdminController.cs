﻿using Lezita2.Context;
using Lezita2.Models;
using Microsoft.AspNetCore.Mvc;
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
            return View(products);
        }
        [HttpGet]
        public IActionResult AddProducts()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult AddProducts(Product product)
        {
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
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("GetCategories");
        }
        [HttpPost]
        public IActionResult DeleteCategories(int id)
        {
            var category=_context.Categories.FirstOrDefault(c => c.Id == id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("GetCategories");
        }
    }
}
