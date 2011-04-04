using System.Collections.Generic;

namespace Kento
{
	abstract class ExternalFunction : Function
	{
		string representation;
		public string Representation
		{
			get { return representation; }
			set { representation = value; }
		}
		public ExternalFunction ( string Name )
			: base( new Array(), new CodeBlock( new List<Token>() ) )
		{
			representation = Name;
		}
	}
}
