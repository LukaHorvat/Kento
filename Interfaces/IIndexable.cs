using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	interface IIndexable
	{
		Reference GetReferenceAtIndex ( int Index );
	}
}
