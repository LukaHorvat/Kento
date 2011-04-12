using System;

namespace Kento
{
	internal class SLSystem : ILibrarySegment
	{
		#region ILibrarySegment Members

		public ExternalClass Load()
		{
			var getTime = new ExternalFunction("GetTime", true, GetTime);

			return new ExternalClass("System", InstanceFlags.NoFlags, getTime);
		}

		#endregion

		public Value GetTime(Array Arguments)
		{
			if (Arguments.Arr.Count > 0) throw new Exception("GetTime doesn't take parameters");
			return new Number(Compiler.RunningTime);
		}
	}
}