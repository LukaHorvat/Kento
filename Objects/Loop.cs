using System.Collections.Generic;
using System.Linq;

namespace Kento
{
	class Loop : CodeBlock
	{
		CodeBlock condition;
		public Loop ( CodeBlock Condition, CodeBlock Block )
			: base( Block.Value )
		{
			condition = Condition;
			Type = CodeBlockType.Loop;
		}
		public override List<Token> Tokenize ()
		{
			return new Token[] { this, new RunCodeBlock() }.ToList();
		}
		public override Value Run ()
		{
			Compiler.EnterScope();
			while ( ( (Boolean)condition.Run() ).Val )
			{
				base.Run();
			}
			Compiler.ExitScope();
			return NoValue.Value;
		}
	}
}
