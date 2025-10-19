using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Concrete
{
    
    public class LogDetail : IEntity
    {
        public int Id { get; set; }
        public string MethodName { get; set; }
        public string User { get; set; }
        public string Parameters { get; set; }
        public DateTime Date { get; set; }
    }
}
