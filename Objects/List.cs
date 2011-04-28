using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	public class List : Value
	{
		static List empty = new List();
		public static List Empty { get { return empty; } }

		public List ( params Value[] Values )
			: this( Values.ToList() ) { }

		public List ()
			: this( new List<Value>() ) { }

		public List ( List<Value> List )
		{
			Arr = List;
		}

		public List<Value> Arr { get; set; }

		public Array ToArray ()
		{
			return new Array( this );
		}

		public override List ToList ()
		{
			return this;
		}

		public Value GetValue ( int Index )
		{
			if ( Arr[ Index ] is Reference ) return ( (Reference)Arr[ Index ] ).ReferencingValue;
			return Arr[ Index ];
		}

		public List<Value> GetValues ()
		{
			return Arr.Select( X => X is Reference ? ( X as Reference ).ReferencingValue : X ).ToList();
		}

		public override string ToString ()
		{
			var builder = new StringBuilder();
			builder.Append( "L[" );
			for ( int i = 0 ; i < Arr.Count ; ++i )
			{
				builder.Append( Arr[ i ].ToString() );
				if ( i < Arr.Count - 1 ) builder.Append( ", " );
			}
			builder.Append( "]" );
			return builder.ToString();
		}
	}
}