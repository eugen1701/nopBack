using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Service.CustomExceptions
{
    public class RegistrationException : Exception
    {
        public RegistrationException()
        {
        }

        public RegistrationException(string message)
            : base(message)
        {
        }

        public RegistrationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
