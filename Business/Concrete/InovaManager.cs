using Business.Abstract;
using Business.Constants;
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
    internal class InovaManager : IInovaService
    {

        IInovaDal _inovadal;
        public InovaManager(IInovaDal inovadal)
        {
            _inovadal = inovadal;
        }
        public IResult Add(Inova inova)
        {
            _inovadal.Add(inova);
            return new SuccessResult(Messages.InovaAdded);
        }

        public IDataResult<List<Inova>> GetAll()
        {
            return new SuccessDataResult<List<Inova>>(_inovadal.GetAll(),Messages.InovasListed);
        }

        public IResult Update(Inova inova)
        {
            throw new NotImplementedException();
        }
    }
}
