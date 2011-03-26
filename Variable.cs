﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	abstract class Variable : Value
	{
	}
	class NoValue : Value
	{
		public override Value Evaluate ()
		{
			return new NoValue();
		}

		public override List<Token> Tokenize ()
		{
			return new List<Token>( new Token[] { (Token)this } );
		}
	}
	class ConditionNotMet : Variable
	{
		public override Value Evaluate ()
		{
			return new NoValue();
		}

		public override List<Token> Tokenize ()
		{
			return new List<Token>( new Token[] { (Token)this } );
		}
	}
}
