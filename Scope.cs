using System.Collections;
using System.Collections.Generic;

namespace Kento
{
	public class Scope : IEnumerable<KeyValuePair<string, Reference>>
	{
		private readonly Dictionary<string, Reference> identifiers;

		public Scope(Dictionary<string, Reference> Identifiers)
		{
			identifiers = Identifiers;
		}

		public Scope()
			: this(new Dictionary<string, Reference>()) {}

		public Reference this[string Key]
		{
			get { return identifiers[Key]; }
			set { identifiers[Key] = value; }
		}

		#region IEnumerable<KeyValuePair<string,Reference>> Members

		public IEnumerator<KeyValuePair<string, Reference>> GetEnumerator()
		{
			return identifiers.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return identifiers.GetEnumerator();
		}

		#endregion

		public Scope Clone()
		{
			var newDict = new Dictionary<string, Reference>();
			foreach (var pair in identifiers)
			{
				newDict[pair.Key] = (pair.Value.Clone() as Reference);
			}
			return new Scope(newDict);
		}

		public bool ContainsKey(string Name)
		{
			return identifiers.ContainsKey(Name);
		}
	}
}