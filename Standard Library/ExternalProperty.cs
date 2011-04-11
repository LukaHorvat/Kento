using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	delegate Value EvaluateBinding ();

	class ExternalProperty : ExternalMember
	{
		readonly EvaluateBinding getter;
		public ExternalProperty ( string Name, bool Static, EvaluateBinding Getter )
		{
			this.Static = Static;
			Representation = Name;
			getter = Getter;
		}
		public override Value Evaluate ()
		{
			var toReturn = getter();
			toReturn.Static = Static;
			return toReturn;
		}
	}
}
