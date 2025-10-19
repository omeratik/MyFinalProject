using Business.Abstract;
using Business.Constants;
using Core.Utilities.Business;
using Core.Utilities.FileHelper;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Business.Concrete
{
    internal class ProductImageManager : IProductImageService
    {
        IProductImageDal _productImageDal;

        public ProductImageManager(IProductImageDal productImageDal)
        {
            _productImageDal = productImageDal;
        }

        public IResult Add(IFormFile file, ProductImage productImage)
        {
            try
            {
                var result = BusinessRules.Run(CheckProductImageLimit(productImage));
                if (result != null)
                {
                    return result;
                }

                productImage.ImagePath = FileHelper.AddAsync(file);
                _productImageDal.Add(productImage);
                return new SuccessResult(Messages.ProductImageAdded);
            }
            catch (DbUpdateException dbEx)
            {
                // Inner exception detayını loglayabiliriz
                return new ErrorResult($"Resim ekleme hatası: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Resim ekleme hatası: {ex.Message}");
            }
        }

        public IResult Delete(ProductImage productImage)
        {
            var oldpath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\wwwroot")) + _productImageDal.Get(I => I.Id == productImage.Id).ImagePath;
            var result = BusinessRules.Run(FileHelper.DeleteAsync(oldpath));
            if (result != null)
            {
                return result;
            }

            _productImageDal.Delete(productImage);
            return new SuccessResult(Messages.CarImageDeleted);


        }

        public IDataResult<List<ProductImage>> GetAll(Expression<Func<ProductImage, bool>> filter = null)
        {
            var result = filter == null 
                ? _productImageDal.GetAll()
                : _productImageDal.GetAll(filter);
            
            return new SuccessDataResult<List<ProductImage>>(result);
        }

        public IDataResult<ProductImage> GetById(int id)
        {
            var result = _productImageDal.Get(p => p.Id == id);
            if (result == null)
            {
                return new ErrorDataResult<ProductImage>(Messages.ProductImageNotFound);
            }
            return new SuccessDataResult<ProductImage>(result);
        }

        public IResult Update(IFormFile file, ProductImage productImage)
        {
            var oldpath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\wwwroot")) + _productImageDal.Get(p => p.Id == productImage.Id).ImagePath;
            productImage.ImagePath = FileHelper.UpdateAsync(oldpath, file);
            _productImageDal.Update(productImage);
            return new SuccessResult();
        }

        private IResult CheckProductImageLimit(ProductImage productImage)
        {
            if (_productImageDal.GetAll(c => c.ProductId == productImage.ProductId).Count >= 5)
            {
                return new ErrorResult(Messages.FailedProductImageAdd);
            }
            return new SuccessResult();
        }





    }
}
