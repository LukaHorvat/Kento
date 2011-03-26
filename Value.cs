using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	abstract class Value : Token
	{
		public virtual Value Evaluate ()
		{
			return this;
		}
		public virtual Array ToArray ()
		{
			return new Array( this );
		}
		public abstract List<Token> Tokenize ();
	}
}
