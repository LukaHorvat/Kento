using System.Collections.Generic;

namespace Kento
{
	class Instance : Value
	{
		Dictionary<string, Reference> identifiers;
		public Dictionary<string, Reference> Identifiers
		{
			get { return identifiers; }
			set { identifiers = value; }
		}

		public Instance ( Type Type )
		{
			identifiers = Type.Identifiers.Clone();
			RewireFunctions();
		}
		public Instance ( Dictionary<string, Reference> Identifiers )
		{
			identifiers = Identifiers.Clone();
			RewireFunctions();
		}
		void RewireFunctions ()
		{
			foreach ( var pair in identifiers )
			{
				if ( pair.Value.ReferencingValue is Function )
				{
					( pair.Value.ReferencingValue as Function ).Scope = identifiers;
				}
			}
		}
		public override Value Clone ()
		{
			return new Instance( identifiers );
		}
	}
}
