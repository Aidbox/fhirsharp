using System;
using System.Collections.Generic;
using System.Text;

namespace Aidbox.RestClient.Exceptions
{
    public class AuthenticationFailed : Exception
    {
        public AuthenticationFailed() : base() { }
        public AuthenticationFailed(string message) : base(message) { }
    }
}
