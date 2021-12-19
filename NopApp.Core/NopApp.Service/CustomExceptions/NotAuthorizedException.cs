using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Service.CustomExceptions
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException()
        {
        }

        public NotAuthorizedException(string message)
            : base(message)
        {
        }

        public NotAuthorizedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}