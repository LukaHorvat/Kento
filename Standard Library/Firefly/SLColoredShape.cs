using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FireflyGL;

namespace Kento
{
	class SLColoredShape : Instance, ILibrarySegment
	{
		public Array FilledPoly { get; set; }
		public Array OutlinePoly { get; set; }
		public ColoredShape Shape { get; set; }

		public SLColoredShape ()
			: base( InstanceFlags.NoFlags ) { }

		public ExternalClass Load ()
		{
			return new ExternalClass( "ColoredShape", InstanceFlags.NoFlags, typeof( SLColoredShape ) );
		}
		public override Value Invoke ( Array Arguments )
		{
			Shape = new ColoredShape();
			Firefly.AddToRenderList( Shape );
			FilledPoly = new Array();
			OutlinePoly = new Array();
			Identifiers[ "FilledPolygons" ] = new Reference( FilledPoly );
			Identifiers[ "OutlinePolygons" ] = new Reference( OutlinePoly );
			Identifiers[ "SetPolygons" ] = new Reference( new ExternalFunction( "SetPolygons", false, SetPolygons ) );
			Identifiers[ "X" ] = new Reference( new ExternalProperty( "X", false, XChange, () => new Number( Shape.X ) ) );
			Identifiers[ "Y" ] = new Reference( new ExternalProperty( "Y", false, YChange, () => new Number( Shape.Y ) ) );
			Identifiers[ "Rotation" ] = new Reference( new ExternalProperty( "Rotation", false, X => Shape.Rotation = (float)( (Number)X ).Val, () => new Number( Shape.Rotation ) ) );

			return this;
		}
		public void XChange ( Value NewValue )
		{
			Shape.X = (float)( (Number)NewValue ).Val;
		}
		public void YChange ( Value NewValue )
		{
			Shape.Y = (float)( (Number)NewValue ).Val;
		}
		public Value SetPolygons ( Array Arguments )
		{
			var fillPolys = FilledPoly.ToArray<SLPolygon>();
			var outlinePolys = OutlinePoly.ToArray<SLPolygon>();

			Shape.FilledPolygons = new LinkedList<Polygon>( fillPolys.Select( X => X.Polygon ) );
			Shape.OutlinePolygons = new LinkedList<Polygon>( outlinePolys.Select( X => X.Polygon ) );
			Shape.SetPolygons();

			return NoValue.Value;
		}
		public override Value Clone ()
		{
			return this;
		}
	}
}
