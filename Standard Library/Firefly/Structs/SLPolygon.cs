using System;
using System.Collections.Generic;
using FireflyGL;

namespace Kento
{
	class SLPolygon : Instance, ILibrarySegment
	{
		public SLPolygon ()
			: base( InstanceFlags.NoFlags ) { }

		public Polygon Polygon { get; set; }

		public ExternalClass Load ()
		{
			return new ExternalClass( "Polygon", InstanceFlags.NoFlags, typeof( SLPolygon ) );
		}
		public override Value Invoke ( List Arguments )
		{
			var list = Arguments.GetValues();
			if ( list[ 0 ] is Boolean )
			{
				var temp = new List<float>();
				for ( int i = 1 ; i < list.Count ; ++i )
				{
					if ( list[ i ] is Number ) temp.Add( (float)( (Number)list[ i ] ).Val );
				}
				Polygon = new Polygon( ( (Boolean)list[ 0 ] ).Val, temp.ToArray() );
			} else throw new Exception( "First argument of the polygon constructor must be a boolean" );

			return this;
		}
		public override Value Clone ()
		{
			var toReturn = new SLPolygon { Polygon = Polygon };
			return toReturn;
		}
	}
}
