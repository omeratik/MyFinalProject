using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.CCS;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Http;

namespace Business.DepencenyResolvers.Autofac;

public class AutofacBusinessModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance(); //Biri senden IProductService isterse ona Product Manager instance ver demek.
		builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();

		builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
		builder.RegisterType<EfCategoryDal>().As<ICategoryDal>().SingleInstance();

		builder.RegisterType<AuthManager>().As<IAuthService>();
		builder.RegisterType<JwtHelper>().As<ITokenHelper>();

		builder.RegisterType<UserManager>().As<IUserService>();
		builder.RegisterType<EfUserDal>().As<IUserDal>();

		var assembly = System.Reflection.Assembly.GetExecutingAssembly();

		builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
			.EnableInterfaceInterceptors(new ProxyGenerationOptions()
			{
				Selector = new AspectInterceptorSelector()
			}).SingleInstance();



	}

}