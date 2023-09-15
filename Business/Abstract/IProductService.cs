using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
	public interface IProductService
	{
		//Business hem entitiy hem de access katmanlarını kullanılır project referance den entitiy ve access katmanlarını seçmemiz gerekiyor.
		IDataResult<List<Product>> GetAll();
		IDataResult<List<Product>> GetAllByCategoryId(int id);
		IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max);

		IDataResult<List<ProductDetailDto>> GetProductDetails();
		IDataResult<Product> GetById(int productId); //burada liste kullanmıyoruz Id istediğimiz icin.
		IResult Add(Product product);
		
	}
}
