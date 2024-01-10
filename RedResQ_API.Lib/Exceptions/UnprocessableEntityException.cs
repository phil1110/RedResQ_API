using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Exceptions
{
    public class UnprocessableEntityException : Exception
    { 
        public UnprocessableEntityException() : base("Not Found!") { }

        public UnprocessableEntityException(string message) : base(message) { }

        public UnprocessableEntityException(string message, Exception innerException) : base(message, innerException) { }
    }
}
