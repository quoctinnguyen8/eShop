using eShop.Database;
using eShop.Database.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Areas.Admin.Controllers
{
	public class ProductCategoryController : BaseController
	{
		public ProductCategoryController(AppDbContext db) : base(db)
		{
		}

		public IActionResult Index()
		{
			var data = _db.ProductCategories.ToList();
			return View(data);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(ProductCategory category)
		{
			category.CreatedAt = DateTime.Now;
			category.UpdatedAt = DateTime.Now;
			var data = _db.ProductCategories.Add(category);
			_db.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
	}
}
