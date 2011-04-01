using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kento
{
	public static class ExtensionMethods
	{
		public static Dictionary<string, Value> Clone ( this Dictionary<string, Value> Original )
		{
			var newDict = new Dictionary<string, Value>();
			foreach ( var pair in Original )
			{
				newDict[ pair.Key ] = pair.Value.Clone();
			}
			return newDict;
		}
	}
}
