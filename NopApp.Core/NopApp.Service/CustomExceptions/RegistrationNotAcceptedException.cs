using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Service.CustomExceptions
{
    public class RegistrationNotAcceptedException : Exception
    {
        public RegistrationNotAcceptedException()
        {
        }

        public RegistrationNotAcceptedException(string message)
            : base(message)
        {
        }

        public RegistrationNotAcceptedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
