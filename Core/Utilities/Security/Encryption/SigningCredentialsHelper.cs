using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Encryption
{
	public class SigningCredentialsHelper //Burası apinin kullanması içindir.
	{
		public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)
		{
			return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);//hashing işleminde anahtar olarak bu securitu key kullan
		}
	}
}
