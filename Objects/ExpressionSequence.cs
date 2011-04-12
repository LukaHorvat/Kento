using System.Collections.Generic;

namespace Kento
{
	internal class ExpressionSequence : Value
	{
		private readonly TokenList list;

		public ExpressionSequence(TokenList List)
		{
			list = List;
		}

		public override List<Token> Tokenize()
		{
			var returnList = new List<Token>();
			for (TokenListNode node = list.First; node != null; node = node.Next)
			{
				returnList.AddRange(((Value) node.Value).Tokenize());
			}
			return returnList;
		}
	}
}