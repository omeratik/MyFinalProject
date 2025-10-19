using Business.Constants;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;

using Microsoft.Extensions.DependencyInjection;

namespace Business.BusinessAspects.Autofac
{
	//JWT için yapıyoruz.
	public class SecuredOperation : MethodInterception
	{
		private string[] _roles;
		private IHttpContextAccessor _httpContextAccessor; 

		public SecuredOperation(string roles)
		{
			_roles = roles.Split(','); //bir metni benim belirttiğim karaktere göre ayır ve array e at.
			_httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
		}

		protected override void OnBefore(IInvocation invocation)
		{
			// HTTP context'in boş olmadığından emin ol
			if (_httpContextAccessor.HttpContext == null)
			{
				Console.WriteLine("HATA: HttpContext null! ServiceTool düzgün çalışmıyor olabilir.");
				throw new Exception(Messages.AuthorizationDenied);
			}
			
			// Authorization header'ını kontrol et
			var authHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
			Console.WriteLine($"Gelen Authorization Header: {authHeader}");
			
			// Rol debug bilgisi
			Console.WriteLine($"Gereken roller: {string.Join(", ", _roles)}");
			
			var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
			
			// Role claim'lerini göster
			Console.WriteLine($"Kullanıcının rolleri: {(roleClaims != null ? string.Join(", ", roleClaims) : "Rol bulunamadı")}");
			
			// Tüm claim'leri kontrol et
			Console.WriteLine("Tüm kullanıcı claim'leri:");
			foreach (var claim in _httpContextAccessor.HttpContext.User.Claims)
			{
				Console.WriteLine($"  - {claim.Type}: {claim.Value}");
			}
			
			foreach (var role in _roles)
			{
				if (roleClaims.Contains(role)) //eğer claimler içinde rol varsa devam et
				{
					Console.WriteLine($"Yetkilendirme başarılı: {role} rolüne sahip.");
					return;
				}
			}
			
			Console.WriteLine("YETKİLENDİRME HATASI: Gerekli rollerden hiçbiri bulunamadı!");
			throw new Exception(Messages.AuthorizationDenied); // yoksa bu mesajı fırlat.
		}
	}
}
