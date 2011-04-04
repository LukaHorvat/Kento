using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class ArrayIdentifier : Identifier
	{
		Array arrayReference;
		int index;
		public ArrayIdentifier ( string Name, Array Array, int Index )
			: base( Name )
		{
			arrayReference = Array;
			index = Index;
		}
		public void SetValue ( Value Value )
		{
			if ( index >= 0 && index < arrayReference.Arr.Count )
			{
				arrayReference.Arr[ index ] = Value;
			}
		}
		public override Value Evaluate ()
		{
			if ( index >= 0 && index < arrayReference.Arr.Count )
			{
				return arrayReference.Arr[ index ];
			} else return NoValue.Value;
		}
	}
}
