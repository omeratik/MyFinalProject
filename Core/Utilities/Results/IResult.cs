using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
	//Temel voidler için başlangıç
	public interface IResult
	{
		bool Success { get; }  //Yapmaya çalıştığın işlem başarılı mı başarısız mı ?
		string Message { get; } // İşlem başarılı, işlem başarısız mesajı gönderme.
	}
}
