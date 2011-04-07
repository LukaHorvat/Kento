using System.Collections.Generic;
using System.Linq;

namespace Kento
{
	public class Array : Value, IInstance
	{
		public Scope Identifiers { get; set; }
		List<Reference> arr;
		public List<Reference> Arr
		{
			get { return arr; }
			set { arr = value; }
		}
		public Array ( params Reference[] Values )
		{
			arr = new List<Reference>( Values );
		}
		public Array ( List<Reference> List )
		{
			arr = List;
		}
		public Array ()
		{
			arr = new List<Reference>();
		}
		public override Array ToArray ()
		{
			return this;
		}
		public override Value Clone ()
		{
			var list = arr.Select( Val => ( Val.Clone() as Reference ) ).ToList();
			return new Array( list );
		}
	}
}
