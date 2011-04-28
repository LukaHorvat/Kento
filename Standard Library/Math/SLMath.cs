using System;

namespace Kento
{
	public class SLMath : ILibrarySegment
	{
		private static readonly Number pi = new Number( Math.PI );
		private static Random random = new Random();

		#region ILibrarySegment Members

		public ExternalClass Load ()
		{
			var piConst = new ExternalProperty( "PI", true, pi );
			var sin = new ExternalFunction( "Sin", true, Sin );
			var cos = new ExternalFunction( "Cos", true, Cos );
			var tan = new ExternalFunction( "Tan", true, Tan );
			var asin = new ExternalFunction( "Asin", true, Asin );
			var acos = new ExternalFunction( "Acos", true, Acos );
			var atan = new ExternalFunction( "Atan", true, Atan );
			var floor = new ExternalFunction( "Floor", true, Floor );
			var ceil = new ExternalFunction( "Ceiling", true, Ceiling );
			var rand = new ExternalFunction( "Random", true, Random );

			return new ExternalClass( "Math", InstanceFlags.NoFlags, piConst, sin, cos, tan, asin, acos, atan, floor, ceil, rand );
		}

		#endregion

		public Value Pi ()
		{
			return pi;
		}

		public Value Sin ( List Arguments )
		{
			if ( Arguments.GetValue( 0 ) is Number )
				return new Number( Math.Sin( ( (Number)Arguments.GetValue( 0 ) ).Val ) );
			throw new Exception( "Sin function takes only a number" );
		}

		public Value Cos ( List Arguments )
		{
			if ( Arguments.GetValue( 0 ) is Number )
				return new Number( Math.Cos( ( (Number)Arguments.GetValue( 0 ) ).Val ) );
			throw new Exception( "Cos function takes only a number" );
		}

		public Value Tan ( List Arguments )
		{
			if ( Arguments.GetValue( 0 ) is Number )
				return new Number( Math.Tan( ( (Number)Arguments.GetValue( 0 ) ).Val ) );
			throw new Exception( "Tan function takes only a number" );
		}

		public Value Asin ( List Arguments )
		{
			if ( Arguments.GetValue( 0 ) is Number )
				return new Number( Math.Asin( ( (Number)Arguments.GetValue( 0 ) ).Val ) );
			throw new Exception( "Asin function takes only a number" );
		}

		public Value Acos ( List Arguments )
		{
			if ( Arguments.GetValue( 0 ) is Number )
				return new Number( Math.Acos( ( (Number)Arguments.GetValue( 0 ) ).Val ) );
			throw new Exception( "Sin function takes only a number" );
		}

		public Value Atan ( List Arguments )
		{
			if ( Arguments.GetValue( 0 ) is Number )
				return new Number( Math.Atan( ( (Number)Arguments.GetValue( 0 ) ).Val ) );
			throw new Exception( "Atan function takes only a number" );
		}

		public Value Floor ( List Arguments )
		{
			if ( Arguments.GetValue( 0 ) is Number )
				return new Number( Math.Floor( ( (Number)Arguments.GetValue( 0 ) ).Val ) );
			throw new Exception( "Floor function takes only a number" );
		}

		public Value Ceiling ( List Arguments )
		{
			if ( Arguments.GetValue( 0 ) is Number )
				return new Number( Math.Ceiling( ( (Number)Arguments.GetValue( 0 ) ).Val ) );
			throw new Exception( "Ceiling function takes only a number" );
		}

		public Value Random ( List Arguments )
		{
			return new Number( random.NextDouble() );
		}
	}
}