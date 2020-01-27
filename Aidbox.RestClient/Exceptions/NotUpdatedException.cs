using System;
using System.Collections.Generic;
using System.Text;

namespace Aidbox.RestClient.Exceptions
{
    public class NotUpdatedException : Exception
    {
        public NotUpdatedException(string message) : base(message) { }
    }
}
