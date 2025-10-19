using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
	public static class ServiceCollectionExtensions
	{
		//Core katmanı dahil ekleyeceğimiz bütün injectionları bir arada toplayabileceğimiz bir yapı yaptık
		//Polimorfizm kullanıldı.
		public static IServiceCollection AddDependencyResolvers(this IServiceCollection serviceCollection,
			ICoreModule[] modules)
		{
			foreach (var module in modules)
			{
				module.Load(serviceCollection);
			}

			ServiceTool.Create(serviceCollection);
			return serviceCollection;
		}
	}
}
