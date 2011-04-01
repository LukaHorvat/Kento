using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class ExpressionSequence : Value
	{
		TokenList list;
		public ExpressionSequence ( TokenList List )
		{
			list = List;
		}
		public override List<Token> Tokenize ()
		{
			var returnList = new List<Token>();
			for ( var node = list.First; node != null ; node = node.Next )
			{
				returnList.AddRange( ( node.Value as Value ).Tokenize() );
			}
			return returnList;
		}
	}
}
