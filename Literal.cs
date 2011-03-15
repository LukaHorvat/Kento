using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	abstract class Literal : Variable
	{
		public abstract object Evaluate ();
	}
}
