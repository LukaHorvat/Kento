namespace Kento
{
	internal delegate Value ProcessInvoke(Array Arguments);

	internal class ExternalFunction : ExternalMember, IFunction
	{
		private readonly ProcessInvoke function;

		public ExternalFunction(string Name, bool Static, ProcessInvoke Function)
		{
			this.Static = Static;
			Representation = Name;
			function = Function;
		}

		#region IFunction Members

		public Value Invoke(Array Arguments)
		{
			return function(Arguments);
		}

		#endregion
	}
}