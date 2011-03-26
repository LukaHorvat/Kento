using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class TokenListNode
	{
		TokenListNode next, previous;
		public TokenListNode Next
		{
			get { return next; }
			set { next = value; }
		}
		public TokenListNode Previous
		{
			get { return previous; }
			set { previous = value; }
		}
		Token value;
		public Token Value
		{
			get { return this.value; }
			set { this.value = value; }
		}
		public TokenListNode ( Token Token )
		{
			value = Token;
		}
		public TokenListNode ()
		{

		}
	}
}
