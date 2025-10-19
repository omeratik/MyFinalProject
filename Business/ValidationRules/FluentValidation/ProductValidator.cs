using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;
using FluentValidation;

namespace Business.ValidationRules.FluentValidation
{
	public class ProductValidator : AbstractValidator<Product>
	{
		public ProductValidator()  //Doğrulama kuralları buraya yazılır.
		{
			RuleFor(p => p.ProductName).MinimumLength(2);
			RuleFor(p => p.ProductName).NotEmpty();
			//RuleFor(p => p.UnitPrice).NotEmpty();
			//RuleFor(p => p.UnitPrice).GreaterThan(0);
			//RuleFor(p => p.UnitPrice).GreaterThanOrEqualTo(10).When(p => p.CategoryId == 1).WithMessage("Unitprice değeri 10 değerinden büyük veya eşit olmalı");
			RuleFor(p => p.ProductName).Must(StartWithUpperCase).WithMessage("Büyük harf kullanınız.");
			


		}

		private bool StartWithUpperCase(string arg)
		{
			char firstChar=arg[0];

			return char.IsUpper(firstChar);


		}
	}
}
