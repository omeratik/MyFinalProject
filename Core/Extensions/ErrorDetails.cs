using FluentValidation.Results;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public class ErrorDetails   //herhangi bir errorda bunları ancak validation error olduğunda altındaki kodları çalıştır ?ValidationErrorDetails
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string DetailedError { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


    //içerisinde validationdetails erroru da içeren 
    public class ValidationErrorDetails : ErrorDetails 
    {
        public IEnumerable<ValidationFailure> Errors { get; set; }

    }
}
