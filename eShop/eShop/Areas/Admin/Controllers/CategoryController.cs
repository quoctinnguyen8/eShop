using eShop.Areas.Admin.ViewModels.Category;
using eShop.Database;
using eShop.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eShop.Areas.Admin.Controllers
{
	public class CategoryController : BaseController
	{
		public CategoryController(AppDbContext db) : base(db)
		{
		}

		public IActionResult Index()
		{
			var query = _db.ProductCategories
						.Select(c => new ListItemCategoryVM
						{
							Id = c.Id,
							Name = c.Name,
							CreatedAt = c.CreatedAt
						})
						.OrderByDescending(c => c.Id);
			ViewBag.Sql = query.ToQueryString();
			var data = query.ToList();
			return View("Index", data);
		}

		public IActionResult Create() => View();

		[HttpPost]
		public IActionResult Create(AddOrUpdateCategoryVM categoryVM)
		{
			// xác thực dữ liệu
			if (ModelState.IsValid == false)
			{
				return View(categoryVM);
			}
			// Lưu vào db
			var category = new ProductCategory();
			category.CreatedAt = DateTime.Now;
			category.UpdatedAt = DateTime.Now;
			category.Name = categoryVM.Name;
			_db.ProductCategories.Add(category);
			_db.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
	}
}
