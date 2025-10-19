using Autofac;
using Autofac.Extras.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Castle.DynamicProxy;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging;
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
        Console.WriteLine("AutofacBusinessModule.Load started");

        builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance(); //Biri senden IProductService isterse ona Product Manager instance ver demek.
		builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();

		builder.RegisterType<CategoryManager>().As<ICategoryService>().SingleInstance();
		builder.RegisterType<EfCategoryDal>().As<ICategoryDal>().SingleInstance();

		builder.RegisterType<AuthManager>().As<IAuthService>();
		builder.RegisterType<JwtHelper>().As<ITokenHelper>();

		builder.RegisterType<UserManager>().As<IUserService>();
		builder.RegisterType<EfUserDal>().As<IUserDal>();

		builder.RegisterType<InovaManager>().As<IInovaService>().SingleInstance();
		builder.RegisterType<EfInovaDal>().As<IInovaDal>().SingleInstance();

		builder.RegisterType<ProductImageManager>().As<IProductImageService>().SingleInstance();
		builder.RegisterType<EfProductImageDal>().As<IProductImageDal>().SingleInstance();
        
		builder.RegisterType<EfLogDal>().As<ILogDal>().SingleInstance();
        builder.RegisterType<DatabaseLogService>().As<ILogService>().SingleInstance();
        Console.WriteLine("Registering DatabaseLogService");
		builder.RegisterType<LogAspect>();

        builder.RegisterType<LogManager>().As<ILogService>().SingleInstance();
		builder.RegisterType<EfLogDal>().As<ILogDal>().InstancePerDependency();

		

		var assembly = System.Reflection.Assembly.GetExecutingAssembly();

		builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
			.EnableInterfaceInterceptors(new ProxyGenerationOptions()
			{
				Selector = new AspectInterceptorSelector()
			}).SingleInstance();
        Console.WriteLine("AutofacBusinessModule.Load completed");


    }

}