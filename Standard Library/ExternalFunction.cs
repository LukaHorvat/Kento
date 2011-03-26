using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	abstract class ExternalFunction : Function
	{
		public ExternalFunction () : base( new Array(), new CodeBlock( new List<Token>() ) ) { }
	}
}
