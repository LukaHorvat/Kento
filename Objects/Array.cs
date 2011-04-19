using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	public class Array : Instance, IIndexable
	{
		private static Array empty = new Array();
		public static Array Empty
		{
			get { return empty; }
		}

		private List<int> dimensions;

		public List<Reference> Arr { get; set; }

		public Array ()
			: this( new List() ) { }

		public Array ( Value Single )
			: this( new List( Single ) ) { }

		public Array ( HardReference HardRef )
			: this( new[] { HardRef }.ToList<Reference>() ) { }

		public Array ( List List )
			: this( List.Arr.Select( X => new Reference( X ) ).ToList() ) { }

		public Array ( List<Reference> List )
			: base( InstanceFlags.Indexable )
		{
			Arr = List;
			Identifiers[ "Count" ] = new Reference( new ExternalProperty( "Count", false, Count ) );
			Identifiers[ "Add" ] = new Reference( new ExternalFunction( "Add", false, Add ) );
			Identifiers[ "PopFirst" ] = new Reference( new ExternalFunction( "PopFirst", false, PopFirst ) );
			Identifiers[ "PopLast" ] = new Reference( new ExternalFunction( "PopLast", false, PopLast ) );
			Identifiers[ "Remove" ] = new Reference( new ExternalFunction( "Remove", false, Remove ) );
			Identifiers[ "RemoveAt" ] = new Reference( new ExternalFunction( "RemoveAt", false, RemoveAt ) );
			Identifiers[ "Insert" ] = new Reference( new ExternalFunction( "Insert", false, Insert ) );
			Identifiers[ "Max" ] = new Reference( new ExternalFunction( "Max", false, Max ) );
			Identifiers[ "Min" ] = new Reference( new ExternalFunction( "Min", false, Min ) );
		}
		#region Members

		private Value PopLast ( Array Arguments )
		{
			var toReturn = Arr.Last();
			Arr.RemoveAt( Arr.Count - 1 );
			return toReturn;
		}

		private Value PopFirst ( Array Arguments )
		{
			var toReturn = Arr.First();
			Arr.RemoveAt( 0 );
			return toReturn;
		}

		public Value Count ()
		{
			return new Number( Arr.Count );
		}

		public Value Add ( Array Arguments )
		{
			for ( int index = 0 ; index < Arguments.Arr.Count ; index++ )
			{
				Reference reference = new Reference( Arguments.Arr[ index ] );
				reference.Accessable = true;
				Arr.Add( reference );
			}
			return this;
		}

		public Value Remove ( Array Arguments )
		{
			for ( int index = 0 ; index < Arguments.Arr.Count ; index++ )
			{
				Reference reference = Arguments.Arr[ index ];
				for ( int i = 0 ; i < Arr.Count ; ++i )
				{
					if ( Arr[ i ].Equals( reference ) )
					{
						Arr.RemoveAt( i );
						--i;
					}
				}
			}
			return this;
		}

		public Value RemoveAt ( Array Arguments )
		{
			for ( int i = 0 ; i < Arguments.Arr.Count ; i++ )
			{
				Reference reference = Arguments.Arr[ i ];
				Value value = reference.ReferencingValue;
				if ( value is Number )
				{
					var index = (int)( value as Number ).Val;
					Arr.RemoveAt( index );
				}
			}
			return this;
		}

		public Value Insert ( Array Arguments )
		{
			Value first = Arguments.Arr[ 0 ].ReferencingValue;
			int index;
			if ( first is Number )
			{
				index = (int)( ( first as Number ).Val );
			} else throw new Exception( "First parameter must be a number" );

			Reference second = Arguments.Arr[ 1 ];
			if ( index >= 0 && index < Arr.Count )
			{
				Arr.Insert( index, second );
			} else throw new Exception( "Index is out of bounds" );
			return this;
		}

		public Value Max ( Array Arguments )
		{
			if ( Arguments.Arr.All( X => X.ReferencingValue is Number ) )
			{
				return Arguments.Arr.Max();
			}
			throw new Exception( "Max function can only be used on arrays containing only numbers" );
		}

		public Value Min ( Array Arguments )
		{
			if ( Arguments.Arr.All( X => X.ReferencingValue is Number ) )
			{
				return Arguments.Arr.Min();
			}
			throw new Exception( "Min function can only be used on arrays containing only numbers" );
		}

		#endregion
		#region IIndexable Members

		public Reference GetReferenceAtIndex ( int Index )
		{
			if ( Index >= 0 && Index < Arr.Count )
				return Arr[ Index ];
			throw new Exception( "Index out of bounds" );
		}

		#endregion
		public override Array ToArray ()
		{
			if ( Arr.Count == 0 ) return this;
			return base.ToArray();
		}

		public List<T> ToArray<T> ()
			where T : Value
		{
			var toReturn = new List<T>();
			foreach ( Reference reference in Arr )
			{
				if ( reference.ReferencingValue is T )
				{
					toReturn.Add( (T)reference.ReferencingValue );
				} else throw new Exception( "Can not convert the array to an array of " + typeof( T ) );
			}
			return toReturn;
		}

		public override Value Clone ()
		{
			var newList = new List<Reference>();
			foreach ( var reference in Arr )
			{
				var value = reference.ReferencingValue;
				newList.Add( new Reference( value is Literal ? value.Clone() : value ) );
			}
			return new Array( newList );
		}

		public override Value Invoke ( Array Arguments )
		{
			dimensions = new List<int>();
			foreach ( Reference dimension in Arguments.Arr )
			{
				if ( dimension.ReferencingValue is Number )
				{
					dimensions.Add( (int)( ( dimension.ReferencingValue as Number ).Val ) );
				} else
				{
					throw new Exception( "Array constructor takes only numbers" );
				}
			}
			List<Reference> toReturn = MakeArray( 0 );
			Arr = toReturn;
			return this;
		}

		private List<Reference> MakeArray ( int Index )
		{
			if ( dimensions.Count == Index )
			{
				var temp = new List<Reference>();
				return temp;
			}
			var array = new List<Reference>();
			int size = dimensions[ Index ];
			for ( int i = 0 ; i < size ; ++i )
			{
				array.Add( new Reference( new Array( MakeArray( Index + 1 ) ) ) );
			}
			return array;
		}

		public override string ToString ()
		{
			var builder = new StringBuilder();
			builder.Append( "[" );
			for ( int i = 0 ; i < Arr.Count ; ++i )
			{
				builder.Append( Arr[ i ].ReferencingValue.ToString() );
				if ( i < Arr.Count - 1 ) builder.Append( ", " );
			}
			builder.Append( "]" );
			return builder.ToString();
		}

		public override void Destroy ()
		{
			foreach ( var reference in Arr )
			{
				reference.Dereference();
			}
		}

		public void LiteralClone ()
		{
			for ( int i = 0 ; i < Arr.Count ; ++i )
			{
				var value = Arr[ i ].ReferencingValue;
				if ( value is Literal )
				{
					Arr[ i ].Dereference();
					Arr[ i ] = new Reference( value.Clone() );
				}
			}
		}
	}
}