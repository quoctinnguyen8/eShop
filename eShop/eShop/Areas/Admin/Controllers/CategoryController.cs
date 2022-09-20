using AutoMapper;
using AutoMapper.QueryableExtensions;
using eShop.Areas.Admin.ViewModels.Category;
using eShop.Database;
using eShop.Database.Entities;
using eShop.WebConfigs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace eShop.Areas.Admin.Controllers
{
	public class CategoryController : BaseController
	{
		private readonly IMapper _mapper;
		public CategoryController(AppDbContext db, IMapper mapper) : base(db)
		{
			_mapper = mapper;
		}

		// Action này sẽ thực thi trước tất cả action khác
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var method = context.HttpContext.Request.Method;

			if (method == HttpMethod.Post.Method)
			{
				if (!ModelState.IsValid)
				{
					// Trả về JSON thông báo lỗi nếu dữ liệu không hợp lệ (JSON dạng array)
					var errorModel = new SerializableError(ModelState);
					context.Result = new BadRequestObjectResult(errorModel);
				}
			}
		}

		public IActionResult Index()
		{
			return View();
		}

		public List<ListItemCategoryVM> GetAll()
		{
			var query = _db.ProductCategories
						.ProjectTo<ListItemCategoryVM>(AutoMapperProfile.categoryAMC)
						.OrderByDescending(c => c.Id);
			var data = query.ToList();
			return data;
		}

		[HttpPost]
		public IActionResult Create([FromBody] AddOrUpdateCategoryVM categoryVM)
		{
			// xác thực dữ liệu
			if (ModelState.IsValid == false)
			{
				return Ok(new
				{
					success = false,
					mesg = "Dữ liệu không hợp lệ, không thể thêm"
				});
			}
			// Lưu vào db
			var category = new ProductCategory();
			category.CreatedAt = DateTime.Now;
			category.UpdatedAt = DateTime.Now;
			_mapper.Map(categoryVM, category);
			_db.ProductCategories.Add(category);
			_db.SaveChanges();
			return Ok(new
			{
				success = true
			});
		}

		[HttpPost]
		public IActionResult Update(int id, [FromBody]AddOrUpdateCategoryVM categoryVM)
		{
			var category = _db.ProductCategories
					.FirstOrDefault(c => c.Id == id);
			category.UpdatedAt = DateTime.Now;
			_mapper.Map(categoryVM, category);
			_db.SaveChanges();
			return Ok(new
			{
				success = true
			});
		}

		public IActionResult Detail(int id)
		{
			var cate = _db.ProductCategories.Find(id);
			var cateVM = _mapper.Map<AddOrUpdateCategoryVM>(cate);
			return Ok(cateVM);
		}

		public IActionResult Delete(int id)
		{
			if (_db.Products.Any(p => p.CategoryId == id))
			{
				// Kiểm tra có sản phẩm nào đang dùng khóa ngoại đến danh mục này không
				return Ok(new
				{
					success = false,
					mesg = "Không thể xóa vì danh mục đã được sử dụng"
				});
			}

			var category = _db.ProductCategories.Find(id);
			_db.Remove(category);
			_db.SaveChanges();
			return Ok(new
			{
				success = true
			});
		}
	}
}
