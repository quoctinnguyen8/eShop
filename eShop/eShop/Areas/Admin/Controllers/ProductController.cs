using AutoMapper;
using AutoMapper.QueryableExtensions;
using eShop.Areas.Admin.ViewModels.Product;
using eShop.Database;
using eShop.WebConfigs;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Areas.Admin.Controllers
{
	public class ProductController : BaseController
	{
		private readonly IMapper _mapper;
		public ProductController(AppDbContext db, IMapper mapper) : base(db)
		{
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult GetAll()
		{
			var data = _db.Products
					.ProjectTo<ListItemProductVM>(AutoMapperProfile.productAMC)
					.ToList();
			return Ok(data);
		}
	}
}
