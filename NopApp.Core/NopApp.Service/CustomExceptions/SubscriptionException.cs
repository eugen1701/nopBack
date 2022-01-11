using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopApp.Service.CustomExceptions
{
    public class SubscriptionException : Exception
    {
        public SubscriptionException()
        {
        }

        public SubscriptionException(string message)
            : base(message)
        {
        }

        public SubscriptionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
