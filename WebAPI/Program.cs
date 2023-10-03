
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Business.Abstract;
using Business.Concrete;
using Business.DepencenyResolvers.Autofac;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			//Autofac, Ninject,CastleWindsor,StructureMap,LightInect,DryInject -->IoC Container Altyapýsý sunarlar.
			//AOP -- Autofac Aop imkaný sunuyor.
			//Postsharp 

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
		



			//builder.Services.AddSingleton<IProductService,ProductManager>();
			//builder.Services.AddSingleton<IProductDal,EfProductDal>();


			//Farklý bir IoC ortamý kullanmak istiyorsak <Autofac> bu syntax ý kullanýrýz.
			builder.Host.UseServiceProviderFactory(services => new AutofacServiceProviderFactory())
				.ConfigureContainer<ContainerBuilder>
				(builder => { builder.RegisterModule(new AutofacBusinessModule()); });



			var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidIssuer = tokenOptions.Issuer,
						ValidAudience = tokenOptions.Audience,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
					};
				});

			//Yarýn Core module gibi farklý moduller oluþturduðumuzda kullanabilmemizi saðlýyor.
			builder.Services.AddDepencenyResolvers(new ICoreModule[]
		{
				new CoreModule()
		});

			var app = builder.Build();
			
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication(); //Keydir
			app.UseAuthorization(); //Burasý keyden sonra ne yapýlacaksa ona izin verendir. Önce Authentication olmasý gerekir.
		

			app.MapControllers();

			app.Run();
		}
	}
}