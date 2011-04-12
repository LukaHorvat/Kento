using System.Collections.Generic;

namespace Kento
{
	internal abstract class Variable : Value {}

	internal class ConditionNotMet : Variable
	{
		public override Value Evaluate()
		{
			return NoValue.Value;
		}

		public override List<Token> Tokenize()
		{
			return new List<Token>(new[]{(Token) this});
		}
	}
}