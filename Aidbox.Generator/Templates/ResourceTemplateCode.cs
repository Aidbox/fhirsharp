using Aidbox.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aidbox.Generator.Templates
{
    partial class ResourceTemplate
    {
		private TypeDescription type;

		public ResourceTemplate(TypeDescription type) 
        { 
			this.type = type;
		}
	}
}
