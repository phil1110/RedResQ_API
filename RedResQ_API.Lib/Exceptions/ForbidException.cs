using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Exceptions
{
    public class ForbidException : Exception
    {
        public ForbidException() : base("Forbidden!") { }

        public ForbidException(string message) : base(message) { }

        public ForbidException(string message, Exception innerException) : base(message, innerException) { }
    }
}
