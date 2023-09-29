using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{

    //generic constraint
    //class : referans tip olabilir demektir.
    //IEntity : IEntity olabilir veya IEntity impelente eden bir nesne olabilir.
    // new() : new'lenebilir olamalı interface ler newlenemez.
    //Core katmanları diğer katmanları referans almaz !
    public interface IEntityRepository<T> where T : class, IEntity, new()
	{

		List<T> GetAll(Expression<Func<T, bool>> filter = null);
		T Get(Expression<Func<T, bool>> filter);

		void Add(T entity);
		void Update(T entity);
		void Delete(T entity);


	}
}
