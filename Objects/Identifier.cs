﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class Identifier : Variable
	{
		string name;
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public Identifier ( string Name )
		{
			name = Name;
		}
		public override string ToString ()
		{
			return Name;
		}
		public override Value Evaluate ()
		{
			if ( Compiler.Runtime )
			{
				return Compiler.GetValue( this );
			} else
			{
				return NoValue.Value;
			}
		}
		public override List<Token> Tokenize ()
		{
			return new List<Token>( new Token[] { (Token)this } );
		}
	}
}
