using System;

namespace Kento
{
	class SLMath : ILibrarySegment
	{
		static Number pi = new Number( Math.PI );

		public ExternalClass Load ()
		{
			var pi = new ExternalProperty( "PI", true, Pi );
			var sin = new ExternalFunction( "Sin", true, Sin );
			var cos = new ExternalFunction( "Cos", true, Cos );
			var tan = new ExternalFunction( "Tan", true, Tan );
			var asin = new ExternalFunction( "Asin", true, Asin );
			var acos = new ExternalFunction( "Acos", true, Acos );
			var atan = new ExternalFunction( "Atan", true, Atan );

			return new ExternalClass( "Math", InstanceFlags.NoFlags, pi, sin, cos, tan, asin, acos, atan );
		}

		public Value Pi ()
		{
			return pi;
		}
		public Value Sin ( Array Arguments )
		{
			if ( Arguments.GetReferenceAtIndex( 0 ).ReferencingValue is Number )
				return new Number( Math.Sin( ( (Number)Arguments.GetReferenceAtIndex( 0 ).ReferencingValue ).Val ) );
			throw new Exception( "Sin function takes only a number" );
		}
		public Value Cos ( Array Arguments )
		{
			if ( Arguments.GetReferenceAtIndex( 0 ).ReferencingValue is Number )
				return new Number( Math.Cos( ( (Number)Arguments.GetReferenceAtIndex( 0 ).ReferencingValue ).Val ) );
			throw new Exception( "Cos function takes only a number" );
		}
		public Value Tan ( Array Arguments )
		{
			if ( Arguments.GetReferenceAtIndex( 0 ).ReferencingValue is Number )
				return new Number( Math.Tan( ( (Number)Arguments.GetReferenceAtIndex( 0 ).ReferencingValue ).Val ) );
			throw new Exception( "Tan function takes only a number" );
		}
		public Value Asin ( Array Arguments )
		{
			if ( Arguments.GetReferenceAtIndex( 0 ).ReferencingValue is Number )
				return new Number( Math.Asin( ( (Number)Arguments.GetReferenceAtIndex( 0 ).ReferencingValue ).Val ) );
			throw new Exception( "Asin function takes only a number" );
		}
		public Value Acos ( Array Arguments )
		{
			if ( Arguments.GetReferenceAtIndex( 0 ).ReferencingValue is Number )
				return new Number( Math.Acos( ( (Number)Arguments.GetReferenceAtIndex( 0 ).ReferencingValue ).Val ) );
			throw new Exception( "Sin function takes only a number" );
		}
		public Value Atan ( Array Arguments )
		{
			if ( Arguments.GetReferenceAtIndex( 0 ).ReferencingValue is Number )
				return new Number( Math.Atan( ( (Number)Arguments.GetReferenceAtIndex( 0 ).ReferencingValue ).Val ) );
			throw new Exception( "Atan function takes only a number" );
		}
	}
}
