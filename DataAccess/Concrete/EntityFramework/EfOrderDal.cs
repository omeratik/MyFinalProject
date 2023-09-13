using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
	public class EfOrderDal : EfEntityRepositoryBase<Order, NorthwindContext>,IOrderDal
	{
		//Database context de(NorthwindContext) DbSet<Order> Orders {get: set: } yapmayı unutma
	}
}
