using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class Type : CodeBlock
	{
		Dictionary<string, Value> identifiers;
		public Dictionary<string, Value> Identifiers
		{
			get { return identifiers; }
			set { identifiers = value; }
		}

		public Type ( Type BaseClass, CodeBlock Code )
			: base( Code.Value )
		{
			identifiers = BaseClass.Identifiers.Clone();
		}
		public Type ( CodeBlock Code )
			: base( Code.Value )
		{
			identifiers = new Dictionary<string, Value>();
		}

		public override Value Run ()
		{
			Compiler.SetAsCurrentScope( identifiers );
			base.Run();
			return this;
		}
	}
}
