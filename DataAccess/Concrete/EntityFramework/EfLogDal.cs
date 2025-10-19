using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfLogDal : EfEntityRepositoryBase<LogDetail, NorthwindContext>, ILogDal
    {
        public override void Add(LogDetail entity)
        {
            try
            {
                using (var context = new NorthwindContext())
                {
                    var addedEntity = context.Entry(entity);
                    addedEntity.State = EntityState.Added;
                    context.SaveChanges();
                    Console.WriteLine("Log entry succesfuly added to database");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in EfLogDal.Add: {ex.Message}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
