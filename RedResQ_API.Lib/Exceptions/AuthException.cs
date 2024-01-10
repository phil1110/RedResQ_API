using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedResQ_API.Lib.Exceptions
{
    public class AuthException : Exception
    {
        public AuthException() : base("Not Authorised!") { }

        public AuthException(string message) : base(message) { }

        public AuthException(string message, Exception innerException) : base(message, innerException) { }
    }
}
