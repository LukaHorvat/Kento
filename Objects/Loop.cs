﻿using System.Collections.Generic;
using System.Linq;

namespace Kento
{
	class Loop : CodeBlock
	{
		public Loop ( CodeBlock Block )
			: base( Block.Value )
		{
			Type = CodeBlockType.Loop;
		}
		public override List<Token> Tokenize ()
		{
			return new Token[] { (Token)this, (Token)new RunCodeBlock() }.ToList();
		}
	}
}
