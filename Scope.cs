using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Kento
{
	public class Scope : IEnumerable<KeyValuePair<string, Reference>>
	{
		private readonly Dictionary<string, Reference> identifiers;
		public LinkedList<Reference> NewReferences { get; set; }
		public bool Blocking { get; set; }	//Determines if identifier search will go to the next scope if the identifier is not found in this one
		//Dot operator scopes use this to prevent function form accessing members of the scope where the function is called from

		public Scope ()
			: this( new Dictionary<string, Reference>() ) { }

		public Scope ( Dictionary<string, Reference> Identifiers )
		{
			identifiers = Identifiers;
			NewReferences = new LinkedList<Reference>();
		}

		public Reference this[ string Key ]
		{
			get { return identifiers[ Key ]; }
			set { identifiers[ Key ] = value; }
		}

		#region IEnumerable<KeyValuePair<string,Reference>> Members

		public IEnumerator<KeyValuePair<string, Reference>> GetEnumerator ()
		{
			return identifiers.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return identifiers.GetEnumerator();
		}

		#endregion

		public void Remove ( string Key )
		{
			identifiers.Remove( Key );
		}

		public Scope Clone ()
		{
			var newDict = new Dictionary<string, Reference>();
			foreach ( var pair in identifiers )
			{
				newDict[ pair.Key ] = Compiler.Reserve( this );
			}
			return new Scope( newDict );
		}

		public bool ContainsKey ( string Name )
		{
			return identifiers.ContainsKey( Name );
		}

		public void Destroy ()
		{
			foreach ( var pair in identifiers )
			{
				pair.Value.Dereference();
			}
		}

		/// <summary>
		/// Sets the parent scope of all functions to this scope so that they can interact with the proper instance of a class
		/// </summary>
		public void RewireFunctions ()
		{
			foreach ( var reference in identifiers.Values.Where( Reference => Reference.ReferencingValue is Function ) )
			{
				( (Function)reference.ReferencingValue ).Scope = this;
			}
		}
	}
}