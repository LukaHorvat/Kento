using System.Collections.Generic;

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
				var reference = Compiler.Identify( Name );
				if ( reference == NullReference.Value )
				{
					reference = new Reference( new NoValue() );
					Compiler.SetAlias( Name, reference );
				}
				return reference;
			}
			return NoValue.Value;
		}
	}
}
