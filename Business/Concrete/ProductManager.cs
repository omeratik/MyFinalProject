using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ValidationRules.FluentValidation;
using Core.CrossCuttingConcerns.Validation;
using FluentValidation;
using Core.Aspects.Autofac.Validation;
using Business.CCS;
using Core.Utilities.Business;
using Business.BusinessAspects.Autofac;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Performance;

namespace Business.Concrete
{
	public class ProductManager : IProductService
	{
		IProductDal _productDal; //Bir entity manager kendisinin hariç başka bir dalı enjekte edemez. Çünki bir başka yerde kural farklı şekilde uygulanması durumunda sürekli olarak tekrardan yazım ve düzenleme gerekicek.
		ICategoryService _categoryService;

		public ProductManager(IProductDal productDal, ICategoryService categoryService)
		{
			_productDal = productDal;
			_categoryService = categoryService;

		}

		// " [LogAspect] "--> AOP Bir metodun önünde ve sonunda bir metod hata verdiğinde, çalışan kod parçacıklarını bu mimari ile yazıyoruz. 



		[ValidationAspect(typeof(ProductValidator))]

		//Claim -- yetkilendirmede "product.add veya admin olması gerekiyor.
		[SecuredOperation("product.add,admin")]
		
		[CacheRemoveAspect("IProductService.Get")]
		public IResult Add(Product product)
		{

			IResult result = BusinessRules.Run(NotSameProductName(product.ProductName),
				CheckIfProductCountOfCategoryCorrect(product.CategoryId), CheckIfCategoryLimitExceded());

			if (result != null) //kurala uymayan durum oluşmuşsa.
			{
				return result;
			}
			//Diğer durumda aşağıdaki kodları çalıştırsın.

			_productDal.Add(product);
			return new SuccessResult(Messages.ProductAdded);

			//      ----BU KISIM İŞ KURALLARININ KÖTÜ YAZILMIŞ HALİ.-----

			//if (CheckIfProductCountOfCategoryCorrect(product.CategoryId).Success)
			//{
			//	if (NotSameProductName(product.ProductName).Success)
			//	{
			//	}

			//}
			//return new ErrorResult();		----BU KISIM İŞ KURALLARININ KÖTÜ YAZILMIŞ HALİ.-----

		}

		[SecuredOperation("product.list,user")]
		[CacheAspect()] //key,value
		public IDataResult<List<Product>> GetAll()
		{
			if (DateTime.Now.Hour == 12)
			{
				return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
			}
			return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);

		}

		public IDataResult<List<Product>> GetAllByCategoryId(int id)
		{
			return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
		}
		[CacheAspect]
		
		public IDataResult<Product> GetById(int productId)
		{
			return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
		}

		public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
		{
			return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
		}

		public IDataResult<List<ProductDetailDto>> GetProductDetails()
		{
			return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
		}


		[ValidationAspect(typeof(ProductValidator))]
		[CacheRemoveAspect("IProductService.Get")]
		[PerformanceAspect(5)]
		public IResult Update(Product product)
		{
			var result = _productDal.GetAll(p => p.CategoryId == product.CategoryId).Count;
			if (result > 25)
			{
				return new ErrorResult(Messages.ProductCountOfCategoryError);
			}
			throw new NotImplementedException();
		}
		

		//iş kuralı parçacığı olduğu icin sadece burada kullanılmalı bu yüzden private olmalı.
		private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
		{
			var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
			if (result > 25)
			{
				return new ErrorResult(Messages.ProductCountOfCategoryError);
			}
			return new SuccessResult();
		}
		private IResult NotSameProductName(string productName)
		{
			var result = _productDal.GetAll(p => p.ProductName == productName).Any();
			if (result)
			{
				return new ErrorResult(Messages.ProductNameAlreadyExsists);
			}
			return new SuccessResult();
		}
		private IResult CheckIfCategoryLimitExceded() //Category service kullanan bir ürünün kuralıdır
		{
			var result = _categoryService.GetAll();
			if (result.Data.Count > 25)
			{
				return new ErrorResult(Messages.CategoryLimitExceded);
			}
			return new SuccessResult();
		}

		[TransactionScopeAspect]
		public IResult AddTransactionalTest(Product product)
		{
			_productDal.Update(product);
			_productDal.Add(product);
			return new SuccessResult(Messages.ProductUpdated);
		}
	}
}
