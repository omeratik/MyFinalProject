using Core.Entities.Concrete;
using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Logging
{
    public interface ILogService
    {
        void Log(LogDetail logDetail);  
        IDataResult<List<LogDetail>> GetAll();
        IDataResult<List<LogDetail>> GetByDateRange(DateTime startDate, DateTime endDate);
        IDataResult<List<LogDetail>> GetByUser(string userName);
    }
}
