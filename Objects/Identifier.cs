using System;

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
					reference = Compiler.Reserve();
					Compiler.SetAlias( Name, reference );
				}
				if ( Compiler.PendingDot )
				{
					Compiler.PendingDot = false;
					Compiler.ExitScope( true );
				}
				return reference;
			}
			return NoValue.Value;
		}
	}
}
