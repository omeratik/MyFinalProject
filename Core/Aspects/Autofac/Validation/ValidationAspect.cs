using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Aspects.Autofac.Validation
{
	public class ValidationAspect : MethodInterception //Aspectimiz = metodun başı sonu veya ortasındaki hata verdiği yerde çalışmasını sitediğimiz yer.
	{
		private Type _validatorType;
		public ValidationAspect(Type validatorType)
		{
			//defensive coding -- attribute da yanlış tip atılmaması için yazılır alt kısım.
			if (!typeof(IValidator).IsAssignableFrom(validatorType))
			{
				throw new System.Exception("Bu bir doğrulama sınıfı değil");
			}

			_validatorType = validatorType;
		}
		protected override void OnBefore(IInvocation invocation)
		{
			var validator = (IValidator)Activator.CreateInstance(_validatorType);
			var entityType = _validatorType.BaseType.GetGenericArguments()[0]; //Doğrulama tipinin base tipini bul onun generic classının 1. sini al demek yani ProductValidator>AbstractValidator<Product>
			var entities = invocation.Arguments.Where(t => t.GetType() == entityType); //Validator typ da belirtilen generic yapının<Product> parametresi ile metodda belirtilen dekinin parametrelerini eşitle.
			foreach (var entity in entities)
			{
				ValidationTool.Validate(validator, entity); //Validatintool kullanarak her birini doğrula
			}
		}
	}
}
