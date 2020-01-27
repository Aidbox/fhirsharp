using Aidbox.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aidbox.Generator.Templates
{
    public partial class TypeTemplate
	{
		protected TypeDescription type;

		public TypeTemplate(TypeDescription type) 
        { 
			this.type = type;
		}		
	}
}
