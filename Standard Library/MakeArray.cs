using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	class MakeArray : ExternalFunction
	{
		List<int> dimensions;
		public MakeArray ()
			: base( "MakeArray" ) { }

		public override Value Invoke ( Array Args )
		{
			dimensions = new List<int>();
			foreach ( var dimension in Args.Arr )
			{
				if ( dimension.ReferencingValue is Number )
				{
					dimensions.Add( (int)( ( dimension.ReferencingValue as Number ).Val ) );
				} else
				{
					throw new Exception( "MakeArray takes only numbers" );
				}
			}
			var toReturn = makeArray( 0 );
			return toReturn;
		}
		Reference makeArray ( int Index )
		{
			if ( dimensions.Count == Index )
			{
				return new Reference( new NoValue() );
			}
			var array = new Array();
			int size = dimensions[ Index ];
			for ( int i = 0 ; i < size ; ++i )
			{
				array.Arr.Add( makeArray( Index + 1 ) );
			}
			return new Reference( array );
		}
	}
}
