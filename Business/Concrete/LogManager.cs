using Business.Constants;
using Core.CrossCuttingConcerns.Logging;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class LogManager : ILogService
    {
        private readonly ILogDal _logDal;

        public LogManager(ILogDal logDal)
        {
            _logDal = logDal;
        }

        public void Log(LogDetail logDetail)
        {
            _logDal.Add(logDetail);
        }

        public IDataResult<List<LogDetail>> GetAll()
        {
            try
            {
                var result = _logDal.GetAll();
                return new SuccessDataResult<List<LogDetail>>(result, "Logs listed successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<LogDetail>>(ex.Message);
            }
        }







            public IDataResult<List<LogDetail>> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = _logDal.GetAll(l => l.Date >= startDate && l.Date <= endDate);
                return new SuccessDataResult<List<LogDetail>>(result, "Logs filtered by date successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<LogDetail>>(ex.Message);
            }
        }

        public IDataResult<List<LogDetail>> GetByUser(string userName)
        {
            try
            {
                var result = _logDal.GetAll(l => l.User == userName);
                return new SuccessDataResult<List<LogDetail>>(result, "Logs filtered by user successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<LogDetail>>(ex.Message);
            }
        }
    }
}
