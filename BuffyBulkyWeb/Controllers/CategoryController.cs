﻿using BuffyBulkyWeb.Data;
using BuffyBulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuffyBulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View();
        }
    }
}
