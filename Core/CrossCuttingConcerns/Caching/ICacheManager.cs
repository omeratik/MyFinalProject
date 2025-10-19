using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching
{
	public interface ICacheManager //bağımsız interface olacak farklı cash teknolojileri kullanmak istersek
	{
		//hangi tipte gelip hangi tipde dönüştüreceğimizi
		T Get<T>(string key);
		//generic olmayan versiyonu
		object Get(string key);
		
		
		void Add(string key, object value, int duration);  //duration= cache de ne kadar süre duracak 
		//void Update(string key, object value, int duration);
		bool IsAdd(string key); //cache de var mı diye kontrol eden yoksa db den çekecek verileri
		void Remove(string key);
		void RemoveByPattern(string pattern);

	}
}
