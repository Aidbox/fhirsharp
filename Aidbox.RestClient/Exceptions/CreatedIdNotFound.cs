using System;
using System.Collections.Generic;
using System.Text;

namespace Aidbox.RestClient.Exceptions
{
    public class CreatedIdNotFound : Exception
    {
        public CreatedIdNotFound(string message) : base(message) { }
    }
}
