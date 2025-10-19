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
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using WebAPI.Hubs;
using WebAPI.SignalR;


namespace WebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //Autofac, Ninject,CastleWindsor,StructureMap,LightInect,DryInject -->IoC Container Altyapı sunarlar.
            //AOP -- Autofac Aop imkanı sunuyor.

            

            builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
            builder.Services.AddCors();
			builder.Services.AddMemoryCache();
			builder.Services.AddHttpContextAccessor();
			
			// SignalR servislerini ekleyin
            builder.Services.AddSignalR(options =>
            {
                // SignalR yapılandırması
                options.EnableDetailedErrors = true;
                options.MaximumReceiveMessageSize = 102400; // 100 KB
            });
            
            // SignalR servisini kaydedin
            builder.Services.AddSingleton<ISignalRService, SignalRService>();
            
			//builder.WebHost.ConfigureKestrel(options =>
			//{
			//options.ListenAnyIP(5000, listenOptions=>{
			//	listenOptions.UseHttps();
			//});
			//});





			//builder.Services.AddSingleton<IProductService,ProductManager>();
			//builder.Services.AddSingleton<IProductDal,EfProductDal>();


			//Farklı bir IoC ortam kullanmak istiyorsak <Autofac> bu syntax kullanırız.
			builder.Host.UseServiceProviderFactory(services => new AutofacServiceProviderFactory())
				.ConfigureContainer<ContainerBuilder>
				(builder => { builder.RegisterModule(new AutofacBusinessModule()); });
			
			
			//builder.WebHost.UseUrls("https://www.atkdepoerp.com:5000");




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
						IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
						RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
						NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
					};
					options.Events = new JwtBearerEvents
					{
						OnTokenValidated = context =>
						{
							var token = context.SecurityToken as JwtSecurityToken;
							Console.WriteLine($"Token doğrulandı. Roller: {string.Join(", ", token.Claims.Where(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Select(c => c.Value))}");
							return Task.CompletedTask;
						},
						OnChallenge = context =>
						{
							Console.WriteLine($"Token doğrulama başarısız: {context.Error ?? "Bilinmeyen Hata"}");
							return Task.CompletedTask;
						}
					};

					// SignalR ve JWT entegrasyonu
					options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            
                            // Path'i kontrol et
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && 
                                (path.StartsWithSegments("/stokHub") || path.StartsWithSegments("/bildirimHub")))
                            {
                                // Token'ı context'e ekle
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
				});

			//Yarın Core module gibi farklı moduller oluşturduğumuzda kullanabilmemizi sağlıyor.
			builder.Services.AddDependencyResolvers(new ICoreModule[]
		{
				new CoreModule()
		});

			var app = builder.Build();

			var autofacContainer = app.Services.GetAutofacRoot();
			ServiceTool.SetContainer(autofacContainer);

			
			// Configure the HTTP request pipeline.
			{
				app.UseSwagger();
				app.UseSwaggerUI();

			}
            
            app.ConfigureCustomExceptionMiddleware(); //her yere try cath yazmak yerine tek at altında topluyoruz.
            app.UseRouting();
			app.UseCors(builder => builder
			  .WithOrigins(
			  "http://localhost:4200",
			    "http://192.168.1.3:4200", 
			   "http://192.168.1.116:4200", 
			    "http://192.168.1.56:4200", 
			    "http://192.168.1.156:4200", 
			    "http://192.168.1.170:4200", 
			    "http://localhost:44317",
			    "https://localhost:5000",
			  	"https://www.atkdepoerp.com",
			  	"https://www.atkdepoerp.com:5000",
			  	"https://api.atkdepoerp.com:5000"
			   )
			  
				.AllowAnyHeader()
				.AllowAnyMethod()
			.AllowCredentials());
			if (!app.Environment.IsDevelopment())
			{
				app.UseHttpsRedirection();
			}


			app.UseAuthentication(); //Keydir
			app.UseAuthorization(); //Burası keyden sonra ne yapılacaksa ona izin verendir. Önce Authentication olması gerekir.

			app.UseStaticFiles();
            app.MapControllers();
			
            // SignalR Hub'larını mapleyin
            app.MapHub<StokHub>("/stokHub");
            app.MapHub<BildirimHub>("/bildirimHub");
            app.MapHub<ProductsHub>("/productsHub");
			
			app.Run();
		}
	}
}