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
		}
	}
}
