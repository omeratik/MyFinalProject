using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Hashing
{
	public  class HashingHelper
	{
		public static void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
		{
			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

			}
		}
		public static bool VerifyPasswordHash(string password,byte[] passwordHash, byte[] passwordSalt)
		 //out kullanmıyoruz çünki biz vereceğiz hash ve saltı.
		{
			using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) //password saltı key istediği için veriyoruz.
			{
				var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
				for (int i = 0; i < computedHash.Length; i++) //Hesaplanan hashler bizim verdiğimiz hash ile uyuşuyor mu ?
				{
					if (computedHash[i] != passwordHash[i]) //eğer uyuşmuyosa false döndür.
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
