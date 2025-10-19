using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.IoC
{
	public static class ServiceTool
	{
		private static ILifetimeScope _lifetimeScope;

		public static IServiceProvider ServiceProvider { get; private set; }

		public static void Create(IServiceCollection services)
		{
			if (services == null)
				throw new ArgumentNullException(nameof(services));

			ServiceProvider = services.BuildServiceProvider();
		}

		public static void SetContainer(ILifetimeScope lifetimeScope)
		{
			_lifetimeScope = lifetimeScope;
		}

		public static object GetService(Type type)
		{
			return _lifetimeScope?.Resolve(type) ?? ServiceProvider.GetService(type);
		}

		public static T GetService<T>() where T : class
		{
			return _lifetimeScope?.Resolve<T>() ?? ServiceProvider.GetService<T>();
		}
	}
}
