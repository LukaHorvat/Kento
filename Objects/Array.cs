using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	public class Array : Instance, ILibrarySegment, IIndexable
	{
		private static Array empty = new Array();
		public static Array Empty { get { return empty; } }

		private List<int> dimensions;

		public List<Reference> Arr { get; set; }

		public Array ()
			: this( new List<Reference>() ) { }

		public Array ( HardReference HardRef )
			: this( new[] { HardRef }.ToList<Reference>() ) { }

		public Array ( List List )
			: this( List.Arr.Select( X => ( new Reference( X is Literal ? X.Clone() : X ) { Accessable = true } ) ).ToList() ) { }

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

		private Value PopLast ( List Arguments )
		{
			var toReturn = Arr.Last();
			Arr.RemoveAt( Arr.Count - 1 );
			return toReturn;
		}

		private Value PopFirst ( List Arguments )
		{
			var toReturn = Arr.First();
			Arr.RemoveAt( 0 );
			return toReturn;
		}

		public Value Count ()
		{
			return new Number( Arr.Count );
		}

		public Value Add ( List Arguments )
		{
			for ( int index = 0 ; index < Arguments.Arr.Count ; index++ )
			{
				Arr.Add( new Reference( Arguments.Arr[ index ] ) { Accessable = true } );
			}
			return this;
		}

		public Value Remove ( List Arguments )
		{
			for ( int index = 0 ; index < Arguments.Arr.Count ; index++ )
			{
#warning FIX THIS METHOD
				var reference = Arguments.Arr[ index ] as Reference;
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

		public Value RemoveAt ( List Arguments )
		{
			for ( int i = 0 ; i < Arguments.Arr.Count ; i++ )
			{
				Value value = Arguments.GetValue( i );
				if ( value is Number )
				{
					var index = (int)( value as Number ).Val;
					Arr.RemoveAt( index );
				}
			}
			return this;
		}

		public Value Insert ( List Arguments )
		{
			Value first = Arguments.GetValue( 0 );
			int index;
			if ( first is Number )
			{
				index = (int)( ( first as Number ).Val );
			} else throw new Exception( "First parameter must be a number" );

			var second = Arguments.GetValue( 1 );
			if ( index >= 0 && index < Arr.Count )
			{
				Arr.Insert( index, new Reference( second ) );
			} else throw new Exception( "Index is out of bounds" );
			return this;
		}

		public Value Max ( List Arguments )
		{
			if ( Arguments.GetValues().All( X => X is Number ) )
			{
				return Arguments.Arr.Max();
			}
			throw new Exception( "Max function can only be used on arrays containing only numbers" );
		}

		public Value Min ( List Arguments )
		{
			if ( Arguments.GetValues().All( X => X is Number ) )
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

		public override List ToList ()
		{
			return new List( Arr.ToList<Value>() );
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
				int index = reference.Index;
				if ( value is Literal )
				{
					newList.Add( new Reference( value.Clone() ) );
				} else
				{
					newList.Add( new Reference( index ) );
				}
			}
			return new Array( newList );
		}

		public override Value Invoke ( List Arguments )
		{
			dimensions = new List<int>();
			foreach ( var dimension in Arguments.GetValues() )
			{
				if ( dimension is Number )
				{
					dimensions.Add( (int)( ( dimension as Number ).Val ) );
				} else
				{
					throw new Exception( "Array constructor takes only numbers" );
				}
			}
			var toReturn = MakeArray( 0 );
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
				builder.Append( Arr[ i ].ToString() );
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
			if ( Identifiers.ContainsKey( "this" ) )
			{
				Compiler.FreeMemoy( Identifiers[ "this" ].Index );
				Identifiers.Remove( "this" );
			}
			Identifiers.Destroy();
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

		#region ILibrarySegment Members

		public ExternalClass Load ()
		{
			return new ExternalClass( "Array", InstanceFlags.Indexable, typeof( Array ) );
		}

		#endregion
	}
}