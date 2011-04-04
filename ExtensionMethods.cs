using System.Collections.Generic;

namespace Kento
{
	public static class ExtensionMethods
	{
		public static Dictionary<string, Reference> Clone ( this Dictionary<string, Reference> Original )
		{
			var newDict = new Dictionary<string, Reference>();
			foreach ( var pair in Original )
			{
				newDict[ pair.Key ] = ( pair.Value.Clone() as Reference );
			}
			return newDict;
		}
	}
}
