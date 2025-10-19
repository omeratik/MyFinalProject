using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Inova:IEntity
    {
        public int InovaId { get; set; }
        public int IrsaliyeId { get; set; }
        public string IrsalıyeNo { get; set; }
        public int SaseId { get; set; }
        public int SaseNo { get; set; }
        public string KoltukTuru { get; set; }
        public string Aksesuarlar { get; set; }
        public int OturakSayisi { get; set; }
        public DateTime Date { get; set; }
    }
}
