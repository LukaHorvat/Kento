using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	public class Array : Instance, IIndexable
	{
		List<int> dimensions;
		List<Reference> arr;

		public List<Reference> Arr
		{
			get { return arr; }
			set { arr = value; }
		}

		public Array ( params Reference[] Values )
			: this( Values.ToList() ) { }

		public Array ()
			: this( new List<Reference>() ) { }

		public Array ( List<Reference> List )
			: base( InstanceFlags.Indexable )
		{
			arr = List;
			Identifiers[ "Count" ] = new Reference( new ExternalProperty( "Count", false, Count ) );
			Identifiers[ "Add" ] = new Reference( new ExternalFunction( "Add", false, Add ) );
			Identifiers[ "Remove" ] = new Reference( new ExternalFunction( "Remove", false, Remove ) );
			Identifiers[ "RemoveAt" ] = new Reference( new ExternalFunction( "RemoveAt", false, RemoveAt ) );
		}
		#region Members
		public Value Count ()
		{
			return new Number( Arr.Count );
		}
		public Value Add ( Array Arguments )
		{
			for ( int index = 0 ; index < Arguments.Arr.Count ; index++ )
			{
				var reference = Arguments.Arr[ index ];
				Arr.Add( reference );
			}
			return this;
		}
		public Value Remove ( Array Arguments )
		{
			for ( int index = 0 ; index < Arguments.Arr.Count ; index++ )
			{
				var reference = Arguments.Arr[ index ];
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
				var reference = Arguments.Arr[ i ];
				var value = reference.ReferencingValue;
				if ( value is Number )
				{
					int index = (int)( value as Number ).Val;
					Arr.RemoveAt( index );
				}
			}
			return this;
		}
		public Value Insert ( Array Arguments )
		{
			for ( int i = 0 ; i < Arguments.Arr.Count ; i++ )
			{
				var reference = Arguments.Arr[ i ];
				var value = reference.ReferencingValue;
				if ( value is Number )
				{
					int index = (int)( value as Number ).Val;
					Arr.RemoveAt( index );
				}
			}
			return this;
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
			foreach ( var reference in Arr )
			{
				if ( reference.ReferencingValue is T )
				{
					toReturn.Add( (T)reference.ReferencingValue );
				} else throw new Exception( "Can not convert the array to an array of " + typeof( T ) );
			}
			return toReturn;
		}
		public Reference GetReferenceAtIndex ( int Index )
		{
			if ( Index >= 0 && Index < arr.Count )
				return arr[ Index ];
			throw new Exception( "Index out of bounds" );
		}
		public override Value Clone ()
		{
			var list = arr.Select( Val => ( Val.Clone() as Reference ) ).ToList();
			return new Array( list );
		}
		public override Value Invoke ( Array Arguments )
		{
			dimensions = new List<int>();
			foreach ( var dimension in Arguments.Arr )
			{
				if ( dimension.ReferencingValue is Number )
				{
					dimensions.Add( (int)( ( dimension.ReferencingValue as Number ).Val ) );
				} else
				{
					throw new Exception( "Array constructor takes only numbers" );
				}
			}
			var toReturn = MakeArray( 0 );
			Arr = toReturn;
			return this;
		}
		List<Reference> MakeArray ( int Index )
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
	}
}
