using Autofac;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

namespace Business.DepencenyResolvers.Autofac;

public class AutofacBusinessModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		builder.RegisterType<ProductManager>().As<IProductService>().SingleInstance(); //Biri senden IProductService isterse ona Product Manager instance ver demek.
		builder.RegisterType<EfProductDal>().As<IProductDal>().SingleInstance();



	}

}