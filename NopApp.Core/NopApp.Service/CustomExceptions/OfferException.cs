using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Service.CustomExceptions
{
    public class OfferException : Exception
    {
        public OfferException()
        {
        }

        public OfferException(string message)
            : base(message)
        {
        }

        public OfferException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
