using Core.Entities.Concrete;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constants
{
	public static class Messages
	{
		public static string ProductAdded = "Ürün eklendi";
		public static string ProductNameInvalid = "Ürün ismi geçersiz";
		public static string MaintenanceTime = "Sistem bakımda";
		public static string ProductsListed = "Ürünler listelendi";

		public static string UnitPriceInvalid = "Ürün yok";
		public static string ProductCountOfCategoryError="Kategoride fazla ürün giremezsin";
		public static string ProductNameAlreadyExsists="Bu isimde zaten başka bir ürün var";
		public static string CategoryLimitExceded="Kategori limiti aşıldığı için yeni ürün eklenemiyor.";
		public static string AuthorizationDenied="Yetkiniz yok.";
		
		public static string UserRegistered="Kullanıcı kayıt oldu.";
		public static string UserNotFound="Kullanıcı bulunamadı";
		public static string PasswordError = "Hatalı parola";
		public static string SuccessfulLogin = "Giriş başarılı";
		public static string UserAlreadyExists = "Kullanıcı zaten var.";
		public static string AccessTokenCreated = "Token olusturuldu.";
	}
}
