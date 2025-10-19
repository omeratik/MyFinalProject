using Core.CrossCuttingConcerns.Logging;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;

public class DatabaseLogService : ILogService
{
    private readonly ILogDal _logDal;

    public DatabaseLogService(ILogDal logDal)
    {
        _logDal = logDal;
    }

    public void Log(LogDetail logDetail)
    {
        try
        {
            Console.WriteLine("DatabaseLogService.Log method called");
            _logDal.Add(logDetail);
            Console.WriteLine("Log successfully added to database");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DatabaseLogService: {ex.Message}");
            throw;
        }
    }

    public IDataResult<List<LogDetail>> GetAll()
    {
        try
        {
            var logs = _logDal.GetAll().OrderByDescending(l => l.Date).ToList();
            return new SuccessDataResult<List<LogDetail>>(logs, "Logs listed successfully");
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
            var logs = _logDal.GetAll(l => l.Date >= startDate && l.Date <= endDate)
                             .OrderByDescending(l => l.Date)
                             .ToList();
            return new SuccessDataResult<List<LogDetail>>(logs, "Logs filtered by date successfully");
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
            var logs = _logDal.GetAll(l => l.User == userName)
                             .OrderByDescending(l => l.Date)
                             .ToList();
            return new SuccessDataResult<List<LogDetail>>(logs, "Logs filtered by user successfully");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<List<LogDetail>>(ex.Message);
        }
    }

    // İsterseniz ek metodlar ekleyebilirsiniz
    public IDataResult<List<LogDetail>> GetByMethod(string methodName)
    {
        try
        {
            var logs = _logDal.GetAll(l => l.MethodName.Contains(methodName))
                             .OrderByDescending(l => l.Date)
                             .ToList();
            return new SuccessDataResult<List<LogDetail>>(logs, "Logs filtered by method successfully");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<List<LogDetail>>(ex.Message);
        }
    }

    public IDataResult<LogDetail> GetById(int id)
    {
        try
        {
            var log = _logDal.Get(l => l.Id == id);
            if (log == null)
                return new ErrorDataResult<LogDetail>("Log not found");

            return new SuccessDataResult<LogDetail>(log, "Log found successfully");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<LogDetail>(ex.Message);
        }
    }

    public IResult Delete(int id)
    {
        try
        {
            var log = _logDal.Get(l => l.Id == id);
            if (log == null)
                return new ErrorResult("Log not found");

            _logDal.Delete(log);
            return new SuccessResult("Log deleted successfully");
        }
        catch (Exception ex)
        {
            return new ErrorResult(ex.Message);
        }
    }
}