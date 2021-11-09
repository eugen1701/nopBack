using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Service.CustomExceptions
{
    public class InvalidRegistrationException : Exception
    {
        public InvalidRegistrationException()
        {
        }

        public InvalidRegistrationException(string message)
            : base(message)
        {
        }

        public InvalidRegistrationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
