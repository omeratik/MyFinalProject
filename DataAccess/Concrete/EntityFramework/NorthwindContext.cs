using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
	//Context : Db tabloları ile proje calasslarını bağlamak
	public class NorthwindContext:DbContext
	{
		// onConfiguring hangi veri tabanını seçeceğimizi belirliyoruz.
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			// Eski bağlantı
			 //optionsBuilder.UseSqlServer(@"Server = OMERLAPTOP\SQLEXPRESS;Database=Northwind;Trusted_Connection=true");

			// Yeni bağlantı - Windows Authentication ile
			optionsBuilder.UseSqlServer(@"Server=OMERLAPTOP\SQLEXPRESS;Database=Northwind;Trusted_Connection=true;TrustServerCertificate=True");

			// VEYA SQL Server Authentication kullanıyorsanız (kullanıcı adı ve şifre ile)
			 //optionsBuilder.UseSqlServer(@"Server=OMERLAPTOP\SQLEXPRESS;Database=Northwind;User Id=sa;Password=123456789/y;TrustServerCertificate=True");
		}
        
		
		public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<OperationClaim> OperationClaims { get; set; }
		public DbSet<UserOperationClaim> UserOperationClaims { get; set; }

		public DbSet<ProductImage> ProductImages { get; set; }
		
		public DbSet<Inova> Inovas {  get; set; }
		public DbSet<LogDetail> LogDetails { get; set; }

	}
}
