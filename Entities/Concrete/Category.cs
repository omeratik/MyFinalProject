using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
	//Çıplak Class Kalmasın -- bir class bir implementasyon almadıysa problem yaşar ileride.
    public class Category:IEntity
	{
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
