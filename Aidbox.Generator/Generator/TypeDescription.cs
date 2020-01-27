using System;
using System.Collections.Generic;
using System.Text;

namespace Aidbox.Generator
{
    public class TypeDescription
    {
        public string EntityName { get; set; }
        public string EntityType { get; set; }
        public List<AttributeDescription> Attributes { get; set; }
    }
}
