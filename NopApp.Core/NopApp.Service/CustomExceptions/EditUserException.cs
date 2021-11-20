using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Service.CustomExceptions
{
    public class EditUserException : Exception
    {
        public EditUserException()
        {
        }

        public EditUserException(string message)
            : base(message)
        {
        }

        public EditUserException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
