using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.SignalR;


namespace WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		//Loosely coupled -- Gevşek bağımlılık
		private readonly IProductService _productService;
		private readonly ISignalRService _signalRService;


		public ProductsController(IProductService productService, ISignalRService signalRService)
		{
			_productService = productService;
			_signalRService = signalRService;
		}


		[HttpGet("getall")]
		public IActionResult GetAll()
		{
			//Swagger
			//Dependency chain --
			//Thread.Sleep(1000);

			var result = _productService.GetAll();
			if (result.Success)
			{
				return Ok(result);
			}

			return BadRequest(result);
		}

		[HttpGet("getbyid")]
		public IActionResult GetById(int id)
		{
			var result = _productService.GetById(id);

			if (result.Success)
			{
				return Ok(result);
			}

			return BadRequest(result);
		}

		[HttpPost("add")]
		public ActionResult Add(Product product)
		{
			try
			{
				var result = _productService.Add(product);
				if (result.Success)
				{
					// Ürün eklendi, SignalR ile diğer istemcilere bildir
					_signalRService.StokDegisiminiBildir(
						product.ProductName, 
						product.UnitsInStock, 
						"Ekleme",
						User.Identity?.Name ?? "Bilinmeyen",
						"Genel"
					);

					return Ok(result);
				}

				return BadRequest(result);
			}
			catch (ValidationException ex)
			{
				Console.WriteLine($"Doğrulama Hatası: {ex.Message}");
				return BadRequest(new { message = $"Doğrulama Hatası: {ex.Message}", success = false });
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Hata: {ex.Message}");
				
				// İç içe exception varsa onları da logla
				Exception innerEx = ex.InnerException;
				while (innerEx != null)
				{
					Console.WriteLine($"İç Hata: {innerEx.Message}");
					innerEx = innerEx.InnerException;
				}
				
				return BadRequest(new { message = $"Bir hata oluştu: {ex.Message}", success = false });
			}
		}
		
		[HttpPost("update")]
        public IActionResult Update([FromBody] ProductDetailDto productDto)
        {
            try
            {
                var result = _productService.Update(productDto);
                if (result.Success)
                {
                    // Ürün güncellendi, SignalR ile diğer istemcilere bildir
                    _signalRService.StokDegisiminiBildir(
                        productDto.ProductName,
                        productDto.UnitsInStock,
                        "Güncelleme",
                        User.Identity?.Name ?? "Bilinmeyen",
                        "Genel"
                    );
                    
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (ValidationException ex)
            {
                Console.WriteLine($"Doğrulama Hatası: {ex.Message}");
                return BadRequest(new { message = $"Doğrulama Hatası: {ex.Message}" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return BadRequest(new { message = $"Bir hata oluştu: {ex.Message}" });
            }
        }

        [HttpGet("getproductdetails")]
        public IActionResult GetProductDetails()
        {
            var result = _productService.GetProductDetails();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbycategory")]
        public IActionResult GetByCategory(int categoryId)
        {
            var result = _productService.GetAllByCategoryId(categoryId);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
		
        [HttpDelete("delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                // Önce ürün bilgisini al (bildirim için)
                var product = _productService.GetById(id);
                if (!product.Success)
                {
                    return BadRequest(product);
                }
                
                // Ürünü sil
                var result = _productService.Delete(id);
                if (result.Success)
                {
                    // Ürün silindi, SignalR ile diğer istemcilere bildir
                    _signalRService.StokDegisiminiBildir(
                        product.Data.ProductName,
                        0,
                        "Silme",
                        User.Identity?.Name ?? "Bilinmeyen",
                        "Genel"
                    );
                    
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return BadRequest(new { message = $"Bir hata oluştu: {ex.Message}" });
            }
        }
    }
}
