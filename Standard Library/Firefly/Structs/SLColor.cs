using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Kento
{
	class SLColor : Instance, ILibrarySegment
	{
		Number red, green, blue;
		public SLColor ()
			: base( InstanceFlags.NoFlags )
		{
		}
		public SLColor ( double Red, double Green, double Blue )
			: this()
		{
			red = new Number( Red );
			green = new Number( Green );
			blue = new Number( Blue );
		}
		public Color GetColor ()
		{
			return Color.FromArgb( (int)( red.Val * 255 ), (int)( green.Val * 255 ), (int)( blue.Val * 255 ) );
		}
		public ExternalClass Load ()
		{
			return new ExternalClass( "Color", InstanceFlags.NoFlags, typeof( SLColor ) );
		}
		public override Value Invoke (Array Arguments)
		{
			if ( Arguments.Arr.Count == 3 )
			{
				Value first = Arguments.Arr[ 0 ].ReferencingValue, second = Arguments.Arr[ 1 ].ReferencingValue, third = Arguments.Arr[ 2 ].ReferencingValue;
				if ( first is Number && second is Number && third is Number )
				{
					red = first as Number;
					green = second as Number;
					blue = third as Number;
					return this;
				}
			}
			throw new Exception( "Color constructor takes 3 numbers" );
		}
		public override Value Clone ()
		{
			return new SLColor( red.Val, green.Val, blue.Val );
		}
	}
}
