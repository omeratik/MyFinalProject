using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		//Loosely coupled -- Gevşek bağımlılık
		IProductService _productService;

		public ProductsController(IProductService productService)
		{
			_productService = productService;
		}


		[HttpGet]
		public List<Product> Get()
		{
			//Dependency chain --
			
			var result= _productService.GetAll();
			return result.Data;
		}
	}
}
