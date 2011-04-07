using System.Collections.Generic;

namespace Kento
{
	class Instance : Value, IInstance
	{
		public Scope Identifiers { get; set; }

		public Instance ( Type Type )
		{
			Identifiers = Type.Identifiers.Clone();
			RewireFunctions();
		}
		public Instance ( Scope Identifiers )
		{
			this.Identifiers = Identifiers.Clone();
			RewireFunctions();
		}
		void RewireFunctions ()
		{
			foreach ( var pair in Identifiers )
			{
				if ( pair.Value.ReferencingValue is Function )
				{
					( pair.Value.ReferencingValue as Function ).Scope = Identifiers;
				}
			}
		}
		public override Value Clone ()
		{
			return new Instance( Identifiers );
		}
	}
}
