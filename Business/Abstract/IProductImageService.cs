using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IProductImageService
    {
        IDataResult<List<ProductImage>> GetAll(Expression<Func<ProductImage, bool>> filter = null);
        IDataResult<ProductImage> GetById(int id);

        IResult Add(IFormFile file, ProductImage image);
        IResult Update(IFormFile file, ProductImage image);

        IResult Delete(ProductImage file); 
    }
}
