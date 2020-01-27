using System;
using System.Collections.Generic;
using System.Text;

namespace Aidbox.RestClient.Exceptions
{
    public class NotCreatedException : Exception
    {
        public NotCreatedException(string message) : base(message) { }
    }
}
